using Microservice.Web.Services;
using StackExchange.Redis;
using System.Text.Json;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _db;
    private readonly IConnectionMultiplexer _multiplexer;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
        _multiplexer = redis;
    }

    // 🔹 Tek veri ekle
    public async Task SetAsync<T>(string key, T value, TimeSpan expiry, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var json = JsonSerializer.Serialize(value);
        await _db.StringSetAsync(key, json, expiry);
    }

    // 🔹 Tek veri getir
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var value = await _db.StringGetAsync(key);

        if (value.IsNullOrEmpty)
            return default;

        return JsonSerializer.Deserialize<T>(value!.ToString());
    }

    // 🔹 Liste ekle
    public async Task SetListAsync<T>(string key, List<T> list, TimeSpan expiry , CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var json = JsonSerializer.Serialize(list);
        await _db.StringSetAsync(key, json, expiry);
    }

    // 🔹 Liste getir
    public async Task<List<T>?> GetListAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var value = await _db.StringGetAsync(key);

        if (value.IsNullOrEmpty)
            return null;

        return JsonSerializer.Deserialize<List<T>>(value!.ToString());
    }

    // 🔹 Sil
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await _db.KeyDeleteAsync(key);
    }

    // 🔹 Desen bazli sil (orn. "personel:*"). SCAN kullanimi: KEYS production'da bloklar.
    public async Task<long> RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        long deleted = 0;
        var database = _db.Database;

        foreach (var endpoint in _multiplexer.GetEndPoints())
        {
            var server = _multiplexer.GetServer(endpoint);
            if (server.IsConnected && !server.IsReplica)
            {
                // Key silme primary uzerinden yapilir; replica'larda taramaya gerek yok.
                var keys = server.Keys(database: database, pattern: pattern, pageSize: 500);

                foreach (var batch in keys.Chunk(100))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    deleted += await _db.KeyDeleteAsync(batch);
                }
            }
        }

        return deleted;
    }

    // 🔹 Var mı kontrolü
    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await _db.KeyExistsAsync(key);
    }

    // 🔹 Expire set et
    public async Task<bool> ExpireAsync(string key, TimeSpan expiry, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await _db.KeyExpireAsync(key, expiry);
    }



    public async Task<bool> AcquireLockAsync(string key, TimeSpan expiry)
    {
        return await _db.StringSetAsync(
            key,
            "1",
            expiry,
            When.NotExists // 🔥 SETNX
        );
    }

    public async Task ReleaseLockAsync(string key)
    {
        await _db.KeyDeleteAsync(key);
    }



}