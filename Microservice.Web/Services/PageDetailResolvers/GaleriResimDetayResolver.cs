using Microservice.Web.Clients.GaleriResimClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Settings;
using Microservice.Web.ViewModels.PageRoute;

namespace Microservice.Web.Services.PageDetailResolvers
{
    public class GaleriResimDetayResolver : IPageDetailResolver
    {
        private readonly IGaleriResimClientServices _galeriResimClient;
        private readonly ILogger<GaleriResimDetayResolver> _logger;

        public GaleriResimDetayResolver(
            IGaleriResimClientServices galeriResimClient,
            ILogger<GaleriResimDetayResolver> logger)
        {
            _galeriResimClient = galeriResimClient;
            _logger = logger;
        }

        public bool CanResolve(PageTypeKindEnum pageType)
        {
            return pageType == PageTypeKindEnum.GaleriResimDetay;
        }

        public async Task<RouteResolveResult?> ResolveAsync(
            RouteResolveResult result,
            string detailSlug)
        {
            var response = await _galeriResimClient
                .GetGaleriResimBySeoUrlAsync(
                    result.Site.Id,
                    result.LanguageId,
                    detailSlug);

            if (!response.IsSuccessful || response.Content is null)
            {
                _logger.LogWarning(
                    "Galeri resmi bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}, SeoUrl: {SeoUrl}",
                    result.Site.Id,
                    result.LanguageId,
                    detailSlug);

                return null;
            }

            result.GaleriResimDetay = response.Content;

            return result;
        }
    }
}
