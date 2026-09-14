using Microservice.Web.Clients.EtkinlikClients;
using Microservice.Web.Clients.HaberClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Settings;
using Microservice.Web.ViewModels.PageRoute;

namespace Microservice.Web.Services.PageDetailResolvers
{
    public class EtkinlikDetayResolver : IPageDetailResolver
    {
        private readonly IEtkinlikClientServices _etkinlikClient;
        private readonly ILogger<EtkinlikDetayResolver> _logger;

        public EtkinlikDetayResolver(
            IEtkinlikClientServices etkinlikClient,
            ILogger<EtkinlikDetayResolver> logger)
        {
            _etkinlikClient = etkinlikClient;
            _logger = logger;
        }

        public bool CanResolve(PageTypeKindEnum pageType)
        {
            return pageType == PageTypeKindEnum.Etkinlik;
        }

        public async Task<RouteResolveResult?> ResolveAsync(
            RouteResolveResult result,
            string detailSlug)
        {
            var response = await _etkinlikClient
                .GetEtkinlikBySeoUrlAsync(
                    result.Site.Id,
                    result.LanguageId,
                    detailSlug);

            if (!response.IsSuccessful || response.Content is null)
            {
                _logger.LogWarning(
                    "Etkinlik bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}, SeoUrl: {SeoUrl}",
                    result.Site.Id,
                    result.LanguageId,
                    detailSlug);

                return null;
            }

            result.EtkinlikDetay = response.Content;

            return result;
        }
    }
}
