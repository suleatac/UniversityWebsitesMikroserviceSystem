using Microservice.Web.Services.Interfaces;
using Microservice.Web.ViewModels.Menu;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Web.ViewComponents
{
    // Sayfa tipinden bağımsız olarak her template layout'unda navbar menülerini üretir.
    public class MenuNavbarViewComponent : ViewComponent
    {
        private readonly IMenuService _menuService;
        private readonly ILogger<MenuNavbarViewComponent> _logger;

        public MenuNavbarViewComponent(IMenuService menuService, ILogger<MenuNavbarViewComponent> logger)
        {
            _menuService = menuService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync(int siteId, int dilId, List<MenuGetVm>? preloadedMenus = null)
        {
            if (siteId <= 0 || dilId <= 0)
            {
                return View(new List<MenuGetVm>());
            }

            List<MenuGetVm> allMenus;

            if (preloadedMenus is not null)
            {
                // Controller aynı istek içinde menüleri zaten çekmişse tekrar servise gitmeyi atla.
                allMenus = preloadedMenus;
            }
            else
            {
                var result = await _menuService.GetMenusAsync(siteId, dilId);

                if (!result.IsSuccess || result.Data is null)
                {
                    _logger.LogWarning("Navbar menüleri alınamadı. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);
                    return View(new List<MenuGetVm>());
                }

                allMenus = result.Data;
            }

            var menus = allMenus
                .Where(m => m.ParentId is null)
                .OrderBy(m => m.Sira)
                .ToList();

            return View(menus);
        }
    }
}
