using Microservice.Web.Clients.DuyuruClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Settings;
using Microservice.Web.ViewModels.PageRoute;

namespace Microservice.Web.Services.PageResolvers
{
    public class DuyuruListesiPageResolver : IPageResolver
    {
        private readonly IDuyuruClientServices _duyuruClient;
        private readonly ILogger<DuyuruListesiPageResolver> _logger;

        public DuyuruListesiPageResolver(
            IDuyuruClientServices duyuruClient,
            ILogger<DuyuruListesiPageResolver> logger)
        {
            _duyuruClient = duyuruClient;
            _logger = logger;
        }

        public bool CanResolve(PageTypeKindEnum pageType)
        {
            return pageType == PageTypeKindEnum.DuyuruListesi;
        }

        public async Task ResolveAsync(RouteResolveResult result)
        {
            var response = await _duyuruClient.GetDuyurularAsync(result.Site.Id, result.LanguageId);

            if (!response.IsSuccessful || response.Content is null)
            {
                _logger.LogWarning(
                    "Duyuru listesi bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}",
                    result.Site.Id,
                    result.LanguageId);

                return;
            }

            result.DuyuruListesi = response.Content;
        }
    }
}
