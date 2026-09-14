using Microservice.Web.Clients.BilgiClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Settings;
using Microservice.Web.ViewModels.PageRoute;

namespace Microservice.Web.Services.PageDetailResolvers
{
    public class BilgiDetayResolver : IPageDetailResolver
    {
        private readonly IBilgiClientServices _bilgiClient;
        private readonly ILogger<BilgiDetayResolver> _logger;

        public BilgiDetayResolver(
            IBilgiClientServices bilgiClient,
            ILogger<BilgiDetayResolver> logger)
        {
            _bilgiClient = bilgiClient;
            _logger = logger;
        }

        public bool CanResolve(PageTypeKindEnum pageType)
        {
            return pageType == PageTypeKindEnum.Bilgi;
        }

        public async Task<RouteResolveResult?> ResolveAsync(
            RouteResolveResult result,
            string detailSlug)
        {
            var response = await _bilgiClient
                .GetBilgiBySeoUrlAsync(
                    result.Site.Id,
                    result.LanguageId,
                    detailSlug);

            if (!response.IsSuccessful || response.Content is null)
            {
                _logger.LogWarning(
                    "Bilgi bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}, SeoUrl: {SeoUrl}",
                    result.Site.Id,
                    result.LanguageId,
                    detailSlug);

                return null;
            }

            result.BilgiDetay = response.Content;

            return result;
        }
    }
}
