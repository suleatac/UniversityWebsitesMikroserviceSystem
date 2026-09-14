using Microservice.Web.Models;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Settings;
using Microservice.Web.ViewModels.PageRoute;
using Microservice.Web.ViewModels.Template;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Microservice.Web.Controllers
{
    public class TemplateController : Controller
    {
        private const int HomeLatestContentCount = 8;

        private readonly IRouteService _routeService;
        private readonly IHaberService _haberService;
        private readonly IDuyuruService _duyuruService;
        private readonly IBannerService _bannerService;
        private readonly IEtkinlikService _etkinlikService;
        private readonly IBilgiService _bilgiService;
        private readonly IShortcutButtonService _shortcutButtonService;
        private readonly IMenuService _menuService;
        private readonly ISiteService _siteService;
        private readonly ILogger<TemplateController> _logger;

        public TemplateController(
            IRouteService routeService,
            IHaberService haberService,
            IDuyuruService duyuruService,
            IBilgiService bilgiService,
            IBannerService bannerService,
            IEtkinlikService etkinlikService,
            IMenuService menuService,
            ISiteService siteService,
            IShortcutButtonService shortcutButtonService,
            ILogger<TemplateController> logger)
        {
            _etkinlikService = etkinlikService;
            _routeService = routeService;
            _bilgiService = bilgiService;
            _haberService = haberService;
            _duyuruService = duyuruService;
            _bannerService = bannerService;
            _menuService = menuService;
            _siteService = siteService;
            _shortcutButtonService = shortcutButtonService;
            _logger = logger;
        }

        /// <summary>
        /// Tüm dinamik template URL'leri buradan karşılanır.
        /// Örnek:
        /// /haberler
        /// /haberler/yeni-laboratuvar
        /// /duyurular
        /// /duyurular/sinav-programi
        /// /akademik-kadro
        /// /iletisim
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //var host = Request.Host.Host;
            var path = Request.Path.Value ?? "/";

            var host = "default.sivas.edu.tr";
            //var path = "/tr/";

            ViewData["Host"] = host;
            ViewData["Path"] = path;
        

            var route = await _routeService.ResolveAsync(
                host,
                path);

            if (route is null)
            {
                return RenderNotFound(
                    $"'{path}' adresi için geçerli bir sayfa bulunamadı.");
     
            }

            return await RenderPageAsync(route);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Sayfa bulunamadığında 404 döndürür ve default error sayfasını gösterir.
        /// </summary>
        private IActionResult RenderNotFound(string? detail = null)
        {
            Response.StatusCode = StatusCodes.Status404NotFound;

            return View("~/Views/Shared/Error.cshtml", new ErrorViewModel {
                Title = "404 - Sayfa Bulunamadı",
                Message = detail ?? "Aradığınız sayfa mevcut değil veya taşınmış olabilir.",
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        /// <summary>
        /// Route sonucundaki PageType'a göre
        /// ilgili sayfanın render edilmesini sağlar.
        /// </summary>
        private async Task<IActionResult> RenderPageAsync(
            RouteResolveResult route)
        {
            var page = route.Page;

            if (page is null)
            {
                return RenderNotFound("Sayfa içeriği bulunamadı.");
            }

            // Navbar menüsü her template sayfasında (Model tipinden bağımsız) bu bilgilerle üretilir.
            ViewData["SiteId"] = route.Site.Id;
            ViewData["DilId"] = route.LanguageId;
            ViewData["LanguageCode"] = route.LanguageCode;
   

            return page.PageTypeKind switch {
                PageTypeKindEnum.Home =>
                     await RenderHomeAsync(route),
                PageTypeKindEnum.HaberListesi when route.DetailSlug is null =>
                     await RenderHaberListesiAsync(route),
                PageTypeKindEnum.HaberDetay  when route.HaberDetay is not null =>
                     await RenderHaberDetayAsync(route),
                PageTypeKindEnum.Etkinlik when route.EtkinlikDetay is not null =>
                     await RenderEtkinlikDetayAsync(route),
                PageTypeKindEnum.Bilgi when route.BilgiDetay is not null =>
                     await RenderBilgiDetayAsync(route),
                PageTypeKindEnum.Menu when route.MenuDetay is not null =>
                     await RenderMenuDetailAsync(route),
                PageTypeKindEnum.DuyuruListesi when route.DetailSlug is null =>
                     await RenderDuyuruListesiAsync(route),
                PageTypeKindEnum.DuyuruDetay when route.DuyuruDetay is not null =>
                     await RenderDuyuruDetayAsync(route),
                PageTypeKindEnum.StaticPage => RenderStaticPage(route),
                _ => RenderNotFound("Bu sayfa türü için tanımlı bir görünüm yok.")
            };
        }

        // ============================================================
        // HOME
        // ============================================================

        /// <summary>
        /// Ana sayfa: menüler + en güncel bannerlar, duyurular ve haberler.
        /// </summary>
        private async Task<IActionResult> RenderHomeAsync(RouteResolveResult route)
        {
            var siteId = route.Site.Id;
            var languageId = route.LanguageId;

            var siteTask = _siteService.GetSiteByIdAsync(siteId);
            var menusTask = _menuService.GetMenusAsync(siteId, languageId);
            var bannersTask = _bannerService.GetBannersAsync(siteId, languageId);
            var haberlerTask = _haberService.GetHabersAsync(siteId, languageId);
            var duyurularTask = _duyuruService.GetDuyurularAsync(siteId, languageId);
            var shortcutButtonsTask = _shortcutButtonService.GetShortcutButtonsAsync(siteId, languageId);
            var bilgiTask = _bilgiService.GetBilgisAsync(siteId, languageId);
            var etkinliklerTask = _etkinlikService.GetEtkinliklerAsync(siteId, languageId);
            await Task.WhenAll(siteTask, menusTask, bannersTask, haberlerTask, duyurularTask, shortcutButtonsTask, bilgiTask, etkinliklerTask);

            var siteResult = await siteTask;

            if (!siteResult.IsSuccess || siteResult.Data is null)
            {
                _logger.LogWarning("Home sayfası için site bulunamadı. SiteId: {SiteId}", siteId);
                return RenderNotFound("Site bulunamadı.");
            }

            var menusResult = await menusTask;
            var bannersResult = await bannersTask;
            var bilgiResult = await bilgiTask;
            var haberlerResult = await haberlerTask;
            var duyurularResult = await duyurularTask;
            var shortcutButtonsResult = await shortcutButtonsTask;
            var etkinliklerResult = await etkinliklerTask;

            var model = new TemplatePageViewModel {
                Site = siteResult.Data,
                // Link'i bos olan icerikler /{LanguageCode}/{PageTypeSlug}/{SeoUrl} adresine yonlendirilir.
                LanguageCode = route.LanguageCode,
                Menus = menusResult.Data ?? [],
                Banners = (bannersResult.Data ?? [])
                    .OrderByDescending(b => b.YayimTarihi)
                    .Take(HomeLatestContentCount)
                    .ToList(),
                Haberler = (haberlerResult.Data ?? [])
                    .OrderByDescending(h => h.YayimTarihi)
                    .Take(HomeLatestContentCount)
                    .ToList(),
                Bilgiler = (bilgiResult.Data ?? [])
                    .OrderByDescending(b => b.YayimTarihi)
                    .Take(HomeLatestContentCount)
                    .ToList(),
                ShortcutButtons= (shortcutButtonsResult.Data ?? [])
                    .OrderByDescending(b => b.Sira)
                    .Take(HomeLatestContentCount)
                    .ToList(),
                Duyurular = (duyurularResult.Data ?? [])
                    .OrderByDescending(d => d.YayimTarihi)
                    .Take(HomeLatestContentCount)
                    .ToList(),
                Etkinlikler = (etkinliklerResult.Data ?? [])
                    .OrderByDescending(e => e.YayimTarihi)
                    .Take(HomeLatestContentCount)
                    .ToList()
            };

            // Navbar component'inin menüleri tekrar servisten çekmesini önlemek için burada paylaşılıyor.
            ViewData["Menus"] = model.Menus;
            ViewData["Site"] = model.Site;
            var viewPath = GetTemplateViewPath(route.Page.TemplateId, "Index");

            return View(viewPath, model);
        }


        // ============================================================
        // MENU
        // ============================================================
        private async Task<IActionResult> RenderMenuDetailAsync(RouteResolveResult route)
        {
            if (route.MenuDetay is null)
            {
                return RenderNotFound("Menü bulunamadı.");
            }

            var model = route.MenuDetay;


            var viewPath = GetTemplateViewPath(route.Page.TemplateId, model.PageType.ViewName);

            return View(viewPath, model);
        }
        // ============================================================
        // ETKİNLİKLER
        // ============================================================

        /// <summary>
        /// /etkinlik/etkinlik-slug
        /// </summary>
        private async Task<IActionResult> RenderEtkinlikDetayAsync(RouteResolveResult route)
        {
            if (route.EtkinlikDetay is null)
            {
                return RenderNotFound("Etkinlik bulunamadı.");
            }

            var model = route.EtkinlikDetay;



            var viewPath = GetTemplateViewPath(route.Page.TemplateId, model.PageType.ViewName);

            return View(viewPath, model);
        }

        // ============================================================
        // BİLGİ SAYFASI
        // ============================================================

        /// <summary>
        /// /BİLGİ/bilgi-slug
        /// </summary>
        private async Task<IActionResult> RenderBilgiDetayAsync(RouteResolveResult route)
        {
            if (route.BilgiDetay is null)
            {
                return RenderNotFound("Bilgi bulunamadı.");
            }

            var model = route.BilgiDetay;



            var viewPath = GetTemplateViewPath(route.Page.TemplateId, model.PageType.ViewName);

            return View(viewPath, model);
        }
        // ============================================================
        // HABERLER
        // ============================================================

        /// <summary>
        /// /haberler
        /// </summary>
        private async Task<IActionResult> RenderHaberListesiAsync(RouteResolveResult route)
        {
            if (route.HaberListesi is null)
            {
                return RenderNotFound("Haberler bulunamadı.");
            }

            var viewPath = GetTemplateViewPath( route.Page.TemplateId,"HaberListesi");

            return View(viewPath, route.HaberListesi);
        }

        /// <summary>
        /// /haberler/haber-slug
        /// </summary>
        private async Task<IActionResult> RenderHaberDetayAsync(RouteResolveResult route)
        {
            if (route.HaberDetay is null)
            {
                return RenderNotFound("Haber bulunamadı.");
            }

            var model = route.HaberDetay;

           

            var viewPath = GetTemplateViewPath(route.Page.TemplateId,model.PageType.ViewName);

            return View(viewPath, model);
        }

        // ============================================================
        // DUYURULAR
        // ============================================================

        /// <summary>
        /// /duyurular
        /// </summary>
        private async Task<IActionResult> RenderDuyuruListesiAsync(RouteResolveResult route)
        {
            if (route.DuyuruListesi is null)
            {
                return RenderNotFound("Duyurular bulunamadı.");
            }

            var viewPath = GetTemplateViewPath(route.Page.TemplateId, "DuyuruListesi");

            return View(viewPath, route.DuyuruListesi);
        }

        /// <summary>
        /// /duyurular/duyuru-slug
        /// </summary>
        private async Task<IActionResult> RenderDuyuruDetayAsync(RouteResolveResult route)
        {
            if (route.DuyuruDetay is null)
            {
                return RenderNotFound("Duyuru bulunamadı.");
            }

            var model = route.DuyuruDetay;



            var viewPath = GetTemplateViewPath(route.Page.TemplateId, model.PageType.ViewName);

            return View(viewPath, model); 
        }

        // ============================================================
        // AKADEMİK KADRO
        // ============================================================

        /// <summary>
        /// /akademik-kadro
        /// </summary>
        // ============================================================
        // STATİK / ÖZEL SAYFALAR
        // ============================================================

        /// <summary>
        /// Örneğin:
        /// /iletisim
        /// /yonetim
        /// /birimler
        /// </summary>
        private IActionResult RenderStaticPage(RouteResolveResult route)
        {
            var viewName = route.Page.ViewName;

            if (string.IsNullOrWhiteSpace(viewName))
            {
                _logger.LogError(
                    "Static sayfada ViewName tanımlı değil. PageId: {PageId}",
                    route.Page.Id);

                return RenderNotFound("Sayfa görünümü tanımlı değil.");
            }

            var viewPath = GetTemplateViewPath(
                route.Page.TemplateId,
                viewName);

            return View(viewPath);
        }

        // ============================================================
        // TEMPLATE VIEW PATH
        // ============================================================

        private static string GetTemplateViewPath(int templateId, string viewName)
        {
            if (templateId <= 0)
            {
                templateId = 1;
            }

            return $"~/Views/Templates/Template{templateId}/{viewName}.cshtml";
        }
    }
}