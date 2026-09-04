using Microservice.Web.Clients.HaberClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Settings;
using Microservice.Web.ViewModels.PageRoute;

namespace Microservice.Web.Services.PageResolvers
{
    public class NewsListPageResolver : IPageResolver
    {
        private readonly IHaberClientServices _haberClient;
        private readonly ILogger<NewsListPageResolver> _logger;

        public NewsListPageResolver(
            IHaberClientServices haberClient,
            ILogger<NewsListPageResolver> logger)
        {
            _haberClient = haberClient;
            _logger = logger;
        }

        public bool CanResolve(PageTypeKindEnum pageType)
        {
            return pageType == PageTypeKindEnum.NewsList;
        }

        public async Task ResolveAsync(RouteResolveResult result)
        {
            var response = await _haberClient.GetHabersAsync(
                result.Site.Id,
                result.LanguageId);

            if (!response.IsSuccessful || response.Content is null)
            {
                _logger.LogWarning(
                    "Haber listesi bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}",
                    result.Site.Id,
                    result.LanguageId);

                return;
            }

            result.NewsList = response.Content;
        }
    }
}