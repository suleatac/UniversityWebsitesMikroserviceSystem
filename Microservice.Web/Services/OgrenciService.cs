using Microservice.Web.Clients.OgrenciClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.ViewModels.Ogrenci;

namespace Microservice.Web.Services
{
    public class OgrenciService : IOgrenciService
    {
        private const string CacheKey = "ogrenci:sayilar:v1";
        private static readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(10);

        private readonly IOgrenciClientServices _ogrenciClient;
        private readonly IRedisCacheService _redisCache;
        private readonly ILogger<OgrenciService> _logger;

        public OgrenciService(
            IOgrenciClientServices ogrenciClient,
            IRedisCacheService redisCache,
            ILogger<OgrenciService> logger)
        {
            _ogrenciClient = ogrenciClient ?? throw new ArgumentNullException(nameof(ogrenciClient));
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<(int Aktif, int Mezun)> GetOgrenciSayilariAsync()
        {
            var cached = await _redisCache.GetAsync<List<OgrenciSayiVm>>(CacheKey);

            if (cached is not null)
            {
                return Calculate(cached);
            }

            List<OgrenciSayiVm> ogrenciler;

            try
            {
                var response = await _ogrenciClient.GetOgrencilerAsync();

                if (!response.IsSuccessStatusCode || response.Content is null)
                {
                    _logger.LogWarning(
                        "Ogrenci listesi alinamadi. StatusCode: {StatusCode}",
                        response.StatusCode);

                    return (0, 0);
                }

                ogrenciler = response.Content;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Ogrenci servisi cagrisinda hata.");
                return (0, 0);
            }

            await _redisCache.SetAsync(CacheKey, ogrenciler, CacheExpiry);

            return Calculate(ogrenciler);
        }

        // Durum alani "mezun" iceren veya MezuniyetTarihi dolu olan kayitlar mezun sayilir.
        private static (int Aktif, int Mezun) Calculate(List<OgrenciSayiVm> ogrenciler)
        {
            var mezun = ogrenciler.Count(o =>
                (o.Durum is not null &&
                 o.Durum.Contains("mezun", StringComparison.OrdinalIgnoreCase)) ||
                o.MezuniyetTarihi is not null);

            return (ogrenciler.Count - mezun, mezun);
        }
    }
}
