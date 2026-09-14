using Microservice.Web.Clients.HaberClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Settings;
using Microservice.Web.ViewModels.PageRoute;

namespace Microservice.Web.Services.PageResolvers
{
    public class HaberListesiPageResolver : IPageResolver
    {
        private readonly IHaberClientServices _haberClient;
        private readonly ILogger<HaberListesiPageResolver> _logger;

        public HaberListesiPageResolver(
            IHaberClientServices haberClient,
            ILogger<HaberListesiPageResolver> logger)
        {
            _haberClient = haberClient;
            _logger = logger;
        }

        public bool CanResolve(PageTypeKindEnum pageType)
        {
            return pageType == PageTypeKindEnum.HaberListesi;
        }

        public async Task ResolveAsync(RouteResolveResult result)
        {
            var response = await _haberClient.GetHabersAsync(result.Site.Id,result.LanguageId);

            if (!response.IsSuccessful || response.Content is null)
            {
                _logger.LogWarning(
                    "Haber listesi bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}",
                    result.Site.Id,
                    result.LanguageId);

                return;
            }

            result.HaberListesi = response.Content;
        }
    }
}