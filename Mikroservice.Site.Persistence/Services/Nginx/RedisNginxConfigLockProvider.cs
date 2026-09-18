using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mikroservice.Site.Persistence.Settings;
using StackExchange.Redis;

namespace Mikroservice.Site.Persistence.Services.Nginx
{
    /// <summary>
    /// nginx yapılandırma yazmalarını Redis üzerinden dağıtık olarak kilitler.
    /// <para>
    /// Birden fazla replika aynı paylaşılan conf.d klasörüne yazdığında (yalnızca yazma değil,
    /// silme ve reconcile işlemleri de) yarış koşulları oluşabilir. Redis erişilemezse
    /// <b>sessizce süreç içi kilide düşer</b>; böylece tek replika senaryosunda sistem
    /// çalışmaya devam eder ve Redis kesintisi site oluşturmayı engellemez.
    /// </para>
    /// </summary>
    public class RedisNginxConfigLockProvider
    (
        IConnectionMultiplexer? connectionMultiplexer,
        IOptions<NginxConfigSetting> settings,
        ILogger<RedisNginxConfigLockProvider> logger
    ) : INginxConfigLockProvider
    {
        private static readonly TimeSpan AcquireRetryDelay = TimeSpan.FromMilliseconds(250);

        // Redis erişilemezse kullanılan süreç içi kili (kaynak adına göre) tutar.
        // Statik olması, servis scope'u değişse bile kilidin paylaşılmasını sağlar.
        private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, SemaphoreSlim> InProcessLocks = new();

        public async Task<IAsyncDisposable?> AcquireAsync(
            string resource,
            TimeSpan leaseTime,
            TimeSpan waitTime,
            CancellationToken cancellationToken = default)
        {
            var lockKey = $"nginx-config:lock:{resource}";
            var deadline = DateTimeOffset.UtcNow.Add(waitTime);

            if (settings.Value.EnableDistributedLock && TryGetDatabase(out var database))
            {
                var ownerToken = Guid.NewGuid().ToString("N");

                while (DateTimeOffset.UtcNow < deadline)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    try
                    {
                        var acquired = await database.StringSetAsync(
                            lockKey,
                            ownerToken,
                            leaseTime,
                            When.NotExists);

                        if (acquired)
                        {
                            logger.LogDebug("Dağıtık kilit alındı: {LockKey}", lockKey);
                            return new RedisLockHandle(database, lockKey, ownerToken, logger);
                        }
                    }
                    catch (RedisException ex)
                    {
                        // Redis kilit sırasında düştüyse süreç içi kilide düş.
                        logger.LogWarning(ex, "Redis kilidi alınamadı, süreç içi kilide geçiliyor: {LockKey}", lockKey);
                        break;
                    }

                    await Task.Delay(AcquireRetryDelay, cancellationToken);
                }

                logger.LogWarning("Dağıtık kilit zaman aşımına uğradı: {LockKey}", lockKey);
                return null;
            }

            return await AcquireInProcessAsync(resource, waitTime, cancellationToken);
        }

        private bool TryGetDatabase(out IDatabase database)
        {
            database = default!;

            if (connectionMultiplexer is null || !connectionMultiplexer.IsConnected)
            {
                return false;
            }

            try
            {
                database = connectionMultiplexer.GetDatabase();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Redis bağlantısı kilitleme için kullanılamıyor.");
                return false;
            }
        }

        private static async Task<IAsyncDisposable?> AcquireInProcessAsync(
            string resource,
            TimeSpan waitTime,
            CancellationToken cancellationToken)
        {
            var semaphore = InProcessLocks.GetOrAdd(resource, _ => new SemaphoreSlim(1, 1));

            var acquired = await semaphore.WaitAsync(waitTime, cancellationToken);
            return acquired ? new InProcessLockHandle(semaphore) : null;
        }

        private sealed class RedisLockHandle : IAsyncDisposable
        {
            // Kilit sahibinin başkasının kilidini silmemesi için Lua ile token karşılaştırması yapılır.
            private const string ReleaseScript = """
                if redis.call('get', KEYS[1]) == ARGV[1] then
                    return redis.call('del', KEYS[1])
                else
                    return 0
                end
                """;

            private readonly IDatabase _database;
            private readonly string _lockKey;
            private readonly string _ownerToken;
            private readonly ILogger _logger;

            public RedisLockHandle(IDatabase database, string lockKey, string ownerToken, ILogger logger)
            {
                _database = database;
                _lockKey = lockKey;
                _ownerToken = ownerToken;
                _logger = logger;
            }

            public async ValueTask DisposeAsync()
            {
                try
                {
                    await _database.ScriptEvaluateAsync(ReleaseScript, new RedisKey[] { _lockKey }, new RedisValue[] { _ownerToken });
                }
                catch (Exception ex)
                {
                    // Kilit lease süresi dolunca kendiliğinden serbest kalır; bu yüzden hata kritik değildir.
                    _logger.LogWarning(ex, "Dağıtık kilit serbest bırakılamadı: {LockKey}", _lockKey);
                }
            }
        }

        private sealed class InProcessLockHandle : IAsyncDisposable
        {
            private readonly SemaphoreSlim _semaphore;

            public InProcessLockHandle(SemaphoreSlim semaphore) => _semaphore = semaphore;

            public ValueTask DisposeAsync()
            {
                _semaphore.Release();
                return ValueTask.CompletedTask;
            }
        }
    }
}
