using Microservice.Web.Clients.DuyuruClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Settings;
using Microservice.Web.ViewModels.PageRoute;

namespace Microservice.Web.Services.PageDetailResolvers
{
    public class AnnouncementDetailResolver : IPageDetailResolver
    {
        private readonly IDuyuruClientServices _duyuruClient;
        private readonly ILogger<AnnouncementDetailResolver> _logger;

        public AnnouncementDetailResolver(
            IDuyuruClientServices duyuruClient,
            ILogger<AnnouncementDetailResolver> logger)
        {
            _duyuruClient = duyuruClient;
            _logger = logger;
        }

        public bool CanResolve(PageTypeKindEnum pageType)
        {
            return pageType == PageTypeKindEnum.AnnouncementList;
        }

        public async Task<RouteResolveResult?> ResolveAsync(
            RouteResolveResult result,
            string detailSlug)
        {
            var response = await _duyuruClient
                .GetDuyuruBySeoUrlAsync(
                    result.Site.Id,
                    result.LanguageId,
                    detailSlug);

            if (!response.IsSuccessful || response.Content is null)
            {
                _logger.LogWarning(
                    "Duyuru bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}, SeoUrl: {SeoUrl}",
                    result.Site.Id,
                    result.LanguageId,
                    detailSlug);

                return null;
            }

            result.AnnouncementDetail = response.Content;

            return result;
        }
    }
}