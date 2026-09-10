using Microservice.Web.Services.Interfaces;
using Microservice.Web.ViewModels.Menu;
using Microservice.Web.ViewModels.Site;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Web.ViewComponents
{
    // Sayfa tipinden bağımsız olarak her template layout'unda navbar menülerini üretir.
    public class MenuNavbarViewComponent : ViewComponent
    {
        // Mikroservice.Site.Domain.Enums.MenuLocation.Header karsiligi
        private const int HeaderLocation = 1;

        private readonly IMenuService _menuService;
        private readonly ILogger<MenuNavbarViewComponent> _logger;

        public MenuNavbarViewComponent(IMenuService menuService, ILogger<MenuNavbarViewComponent> logger)
        {
            _menuService = menuService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            int siteId,
            int dilId,
            List<MenuGetVm>? preloadedMenus = null,
            SiteDetailGetVm? preloadedSite = null)
        {
            if (siteId <= 0 || dilId <= 0)
            {
                return View(new MenuGetIndexVm());
            }

            var menus = preloadedMenus ?? await GetMenusAsync(siteId, dilId);

            // Footer/sidebar menuleri navbar'da gorunmemeli
            var rootMenus = menus
                .Where(menu => menu.ParentId is null
                               && menu.IsVisible
                               && menu.Location == HeaderLocation)
                .OrderBy(menu => menu.Sira)
                .ToList();

            var site = preloadedSite ?? new SiteDetailGetVm {
                Id = siteId
            };

            var viewModel = new MenuGetIndexVm {
                Site = site,
                Menus = rootMenus
            };

            return View(viewModel);
        }

        private async Task<List<MenuGetVm>> GetMenusAsync(int siteId, int dilId)
        {
            var result = await _menuService.GetMenusAsync(siteId, dilId);

            if (result.IsSuccess && result.Data is not null)
            {
                return result.Data;
            }

            _logger.LogWarning(
                "Navbar menüleri alınamadı. SiteId: {SiteId}, DilId: {DilId}",
                siteId,
                dilId);

            return new List<MenuGetVm>();
        }


    }
}
