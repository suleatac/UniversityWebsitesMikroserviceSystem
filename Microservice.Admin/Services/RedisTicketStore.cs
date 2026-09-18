using Microservice.Admin.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Security.Cryptography;

namespace Microservice.Admin.Services
{
    /// <summary>
    /// Cookie kimlik doğrulama ticket'ını (claim'ler + access/refresh token) Redis'te saklar.
    ///
    /// Neden gerekli?
    /// - Varsayılan <see cref="ChunkingCookieManager"/> ticket'ı doğrudan cookie'ye yazar.
    ///   Keycloak access token'ı (1.5-4 KB) 4096 byte'lık tarayıcı cookie limitini aştığı için
    ///   cookie <c>CookieC1</c>, <c>CookieC2</c>... şeklinde parçalanır. Bu durum her istekte
    ///   büyük header'lar oluşturur ve nginx <c>large_client_header_buffers</c> limitini aşarak
    ///   HTTP 400 üretir.
    /// - Bu store ile cookie içinde yalnızca kısa bir anahtar taşınır; token ve claim'ler Redis'te
    ///   tutulur.
    /// </summary>
    public class RedisTicketStore : ITicketStore
    {
        private const string CacheKeyPrefix = "auth:ticket:";

        private readonly IDistributedCache _cache;
        private readonly AuthCookieSetting _settings;
        private readonly ILogger<RedisTicketStore> _logger;

        public RedisTicketStore(
            IDistributedCache cache,
            IOptions<AuthCookieSetting> settings,
            ILogger<RedisTicketStore> logger)
        {
            _cache = cache;
            _settings = settings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Ticket'ı Redis'e yazar ve cookie'de saklanacak kısa anahtarı döndürür.
        /// </summary>
        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var key = GenerateKey();
            await RenewAsync(key, ticket);
            return key;
        }

        /// <summary>
        /// Cookie'deki anahtarla ticket'ı Redis'ten okur. Sliding expiration açıksa
        /// süre ticket üzerindeki ExpiresUtc'e göre uzatılır.
        /// </summary>
        public async Task<AuthenticationTicket?> RetrieveAsync(string key)
        {
            try
            {
                var bytes = await _cache.GetAsync(CacheKeyPrefix + key);

                if (bytes == null || bytes.Length == 0)
                {
                    return null;
                }

                var ticket = TicketSerializer.Default.Deserialize(bytes);

                if (ticket != null && _settings.SlidingExpiration)
                {
                    var ttl = ResolveLifetime(ticket);

                    if (ttl > TimeSpan.Zero)
                    {
                        await _cache.RefreshAsync(CacheKeyPrefix + key);
                    }
                }

                return ticket;
            }
            catch (RedisConnectionException ex)
            {
                // Redis geçici olarak erişilemezse kullanıcıyı "oturum yok" olarak değerlendir.
                // Bu, tüm istekleri 500 ile düşürmekten daha güvenlidir; kullanıcı tekrar giriş yapar.
                _logger.LogError(ex, "Ticket Redis'ten okunamadı. Kullanıcı oturumu geçersiz sayıldı.");
                return null;
            }
        }

        /// <summary>
        /// Ticket'ı Redis'e (yeniden) yazar. Token yenilendiğinde de bu metot kullanılır.
        /// </summary>
        public async Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            var bytes = TicketSerializer.Default.Serialize(ticket);
            var ttl = ResolveLifetime(ticket);

            try
            {
                await _cache.SetAsync(
                    CacheKeyPrefix + key,
                    bytes,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = ttl
                    });
            }
            catch (RedisConnectionException ex)
            {
                _logger.LogError(ex, "Ticket Redis'e yazılamadı.");
            }
        }

        /// <summary>
        /// Ticket'ı Redis'ten siler (sign-out).
        /// </summary>
        public async Task RemoveAsync(string key)
        {
            try
            {
                await _cache.RemoveAsync(CacheKeyPrefix + key);
            }
            catch (RedisConnectionException ex)
            {
                _logger.LogError(ex, "Ticket Redis'ten silinemedi.");
            }
        }

        /// <summary>
        /// Cookie içindeki anahtarın uzunluğunu kısaltır.
        /// </summary>
        private static string GenerateKey() =>
            Base64UrlTextEncoder.Encode(RandomNumberGenerator.GetBytes(16));

        private TimeSpan ResolveLifetime(AuthenticationTicket ticket)
        {
            var buffer = TimeSpan.FromMinutes(Math.Max(_settings.TicketBufferMinutes, 1));
            var defaultLifetime = TimeSpan.FromMinutes(Math.Max(_settings.ExpireMinutes, 1));

            if (ticket.Properties?.ExpiresUtc is { } expiresUtc)
            {
                var remaining = expiresUtc - DateTimeOffset.UtcNow;

                if (remaining <= TimeSpan.Zero)
                {
                    return TimeSpan.Zero;
                }

                return remaining + buffer;
            }

            return defaultLifetime + buffer;
        }
    }
}
