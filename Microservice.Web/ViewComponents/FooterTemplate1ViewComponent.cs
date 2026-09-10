using Microservice.Web.Services.Interfaces;
using Microservice.Web.ViewModels.BandLogo;
using Microservice.Web.ViewModels.Footer;
using Microservice.Web.ViewModels.Haber;
using Microservice.Web.ViewModels.Menu;
using Microservice.Web.ViewModels.Site;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Web.ViewComponents
{
    public class FooterTemplate1ViewComponent : ViewComponent
    {
        // Mikroservice.Site.Domain.Enums.MenuLocation.Footer karsiligi
        private const int FooterLocation = 2;
        private const int LatestHaberCount = 6;

        private readonly ILogger<FooterTemplate1ViewComponent> _logger;
        private readonly IBandLogoService _bandLogoService;
        private readonly IMenuService _menuService;
        private readonly IHaberService _haberService;

        public FooterTemplate1ViewComponent(
            ILogger<FooterTemplate1ViewComponent> logger,
            IBandLogoService bandLogoService,
            IMenuService menuService,
            IHaberService haberService)
        {
            _logger = logger;
            _bandLogoService = bandLogoService;
            _menuService = menuService;
            _haberService = haberService;
        }

        public async Task<IViewComponentResult> InvokeAsync(
           int siteId,
           int dilId,
           SiteDetailGetVm? preloadedSite = null)
        {
            if (siteId <= 0 || dilId <= 0)
            {
                return View(new FooterTemplate1Vm());
            }

            var bandLogosTask = _bandLogoService.GetBandLogosAsync(siteId, dilId);
            var footerMenusTask = _menuService.GetMenusAsync(siteId, dilId, FooterLocation);
            var haberlerTask = _haberService.GetHabersAsync(siteId, dilId);
            await Task.WhenAll(bandLogosTask, footerMenusTask, haberlerTask);

            var site = preloadedSite ?? new SiteDetailGetVm {
                Id = siteId
            };

            var viewModel = new FooterTemplate1Vm {
                Site = site,
                DilId = dilId,
                BandLogos = bandLogosTask.Result.Data ?? new List<GetBandLogoVm>(),
                FooterColumns = FilterFooterColumns(footerMenusTask.Result.Data),
                LatestHaberler = (haberlerTask.Result.Data ?? new List<GetHaberVm>())
                    .OrderByDescending(h => h.YayimTarihi)
                    .Take(LatestHaberCount)
                    .ToList()
            };

            return View(viewModel);
        }

        // Sadece gorunur kok sutunlari ve siralanmis linkleri dondurur
        private List<MenuGetVm> FilterFooterColumns(List<MenuGetVm>? menus)
        {
            if (menus is null)
            {
                return new List<MenuGetVm>();
            }

            return menus
                .Where(menu => menu.ParentId is null && menu.IsVisible)
                .OrderBy(menu => menu.Sira)
                .Select(column => new MenuGetVm {
                    Id = column.Id,
                    SiteId = column.SiteId,
                    DilId = column.DilId,
                    HedefId = column.HedefId,
                    Baslik = column.Baslik,
                    Link = column.Link,
                    Sira = column.Sira,
                    Location = column.Location,
                    IsVisible = column.IsVisible,
                    ParentId = column.ParentId,
                    Children = column.Children
                        .Where(child => child.IsVisible)
                        .OrderBy(child => child.Sira)
                        .ToList()
                })
                .ToList();
        }
    }
}
