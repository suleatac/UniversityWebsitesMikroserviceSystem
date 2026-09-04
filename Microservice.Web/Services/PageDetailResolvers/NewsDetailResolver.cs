using Microservice.Web.Clients.HaberClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Settings;
using Microservice.Web.ViewModels.PageRoute;

namespace Microservice.Web.Services.PageDetailResolvers
{
    public class NewsDetailResolver : IPageDetailResolver
    {
        private readonly IHaberClientServices _haberClient;
        private readonly ILogger<NewsDetailResolver> _logger;

        public NewsDetailResolver(
            IHaberClientServices haberClient,
            ILogger<NewsDetailResolver> logger)
        {
            _haberClient = haberClient;
            _logger = logger;
        }

        public bool CanResolve(PageTypeKindEnum pageType)
        {
            return pageType == PageTypeKindEnum.NewsList;
        }

        public async Task<RouteResolveResult?> ResolveAsync(
            RouteResolveResult result,
            string detailSlug)
        {
            var response = await _haberClient
                .GetHaberBySeoUrlAsync(
                    result.Site.Id,
                    result.LanguageId,
                    detailSlug);

            if (!response.IsSuccessful || response.Content is null)
            {
                _logger.LogWarning(
                    "Haber bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}, SeoUrl: {SeoUrl}",
                    result.Site.Id,
                    result.LanguageId,
                    detailSlug);

                return null;
            }

            result.NewsDetail = response.Content;

            return result;
        }
    }
}