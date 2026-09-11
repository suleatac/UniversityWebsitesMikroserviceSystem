using Microservice.Web.Clients.MenuClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Settings;
using Microservice.Web.ViewModels.PageRoute;

namespace Microservice.Web.Services.PageDetailResolvers
{
    public class MenuDetailResolver: IPageDetailResolver
    {
        private readonly IMenuClientServices _menuClient;
        private readonly ILogger<MenuDetailResolver> _logger;

        public MenuDetailResolver(
            IMenuClientServices menuClient,
            ILogger<MenuDetailResolver> logger)
        {
            _menuClient = menuClient;
            _logger = logger;
        }

        public bool CanResolve(PageTypeKindEnum pageType)
        {
            return pageType == PageTypeKindEnum.Menu;
        }

        public async Task<RouteResolveResult?> ResolveAsync(
            RouteResolveResult result,
            string detailSlug)
        {
            var response = await _menuClient
                .GetMenuBySeoUrlAsync(
                    result.Site.Id,
                    result.LanguageId,
                    detailSlug);

            if (!response.IsSuccessful || response.Content is null)
            {
                _logger.LogWarning(
                    "Menu bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}, SeoUrl: {SeoUrl}",
                    result.Site.Id,
                    result.LanguageId,
                    detailSlug);

                return null;
            }

            result.MenuDetail = response.Content;

            return result;
        }
    }
}
