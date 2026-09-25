using Microservice.Web.Clients.BannerClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Settings;
using Microservice.Web.ViewModels.PageRoute;

namespace Microservice.Web.Services.PageDetailResolvers
{
    /// <summary>
    /// /{dil}/banner/{seoUrl} icin banner detayini cozumler.
    /// Site API'de "seoUrl ile getirme" ucu olmadigindan once site+dyil banner listesi
    /// cekilir, SeoUrl eslesen kaydin Id'si ile detay istenir.
    /// </summary>
    public class BannerDetayResolver : IPageDetailResolver
    {
        private readonly IBannerClientServices _bannerClient;
        private readonly ILogger<BannerDetayResolver> _logger;

        public BannerDetayResolver(
            IBannerClientServices bannerClient,
            ILogger<BannerDetayResolver> logger)
        {
            _bannerClient = bannerClient;
            _logger = logger;
        }

        public bool CanResolve(PageTypeKindEnum pageType)
        {
            return pageType == PageTypeKindEnum.Banner;
        }

        public async Task<RouteResolveResult?> ResolveAsync(
            RouteResolveResult result,
            string detailSlug)
        {
            var listResponse = await _bannerClient
                .GetBannersAsync(result.Site.Id, result.LanguageId);

            var found = listResponse.Content?
                .FirstOrDefault(b => string.Equals(
                    (b.SeoUrl ?? string.Empty).Trim('/'),
                    (detailSlug ?? string.Empty).Trim('/'),
                    StringComparison.OrdinalIgnoreCase));

            if (found is null)
            {
                _logger.LogWarning(
                    "Banner bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}, SeoUrl: {SeoUrl}",
                    result.Site.Id,
                    result.LanguageId,
                    detailSlug);

                return null;
            }

            var detailResponse = await _bannerClient.GetBannerByIdAsync(found.Id);

            if (!detailResponse.IsSuccessful || detailResponse.Content is null)
            {
                _logger.LogWarning(
                    "Banner detayı alınamadı. BannerId: {BannerId}",
                    found.Id);

                return null;
            }

            result.BannerDetay = detailResponse.Content;

            return result;
        }
    }
}
