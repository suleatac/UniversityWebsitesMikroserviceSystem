using Microservice.Web.Models;
using Microservice.Web.Services;
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
        private const int HomeLatestContentCount = 5;

        private readonly IRouteService _routeService;
        private readonly IHaberService _haberService;
        private readonly IDuyuruService _duyuruService;
        private readonly IBannerService _bannerService;
        private readonly IMenuService _menuService;
        private readonly ISiteService _siteService;
        private readonly ILogger<TemplateController> _logger;

        public TemplateController(
            IRouteService routeService,
            IHaberService haberService,
            IDuyuruService duyuruService,
            IBannerService bannerService,
            IMenuService menuService,
            ISiteService siteService,
            ILogger<TemplateController> logger)
        {
            _routeService = routeService;
            _haberService = haberService;
            _duyuruService = duyuruService;
            _bannerService = bannerService;
            _menuService = menuService;
            _siteService = siteService;
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
            //var path = Request.Path.Value ?? "/";
            //var host = "default.sivas.edu.tr";
            //var path =  "/";

            var host = Request.Host.Host;
            var path = Request.Path.Value ?? "/";
            //return View("~/Views/Templates/Template4/Index.cshtml");

            var route = await _routeService.ResolveAsync(
                host,
                path);

            if (route is null)
            {
                return NotFound();
            }

            return await RenderPageAsync(route);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
                return NotFound();
            }

            // Navbar menüsü her template sayfasında (Model tipinden bağımsız) bu bilgilerle üretilir.
            ViewData["SiteId"] = page.SiteId;
            ViewData["DilId"] = route.LanguageId;
            ViewData["LanguageCode"] = route.LanguageCode;

            return (PageType)page.PageTypeId switch {
                PageType.Home =>
                   await RenderHomeAsync(route),
                PageType.NewsList when route.DetailSlug is null =>
                    await RenderNewListAsync(route),
                PageType.New  when route.New is not null =>
                    await RenderNewDetailAsync(route),
                PageType.AnnouncementList when route.DetailSlug is null =>
                    await RenderAnnouncementListAsync(route),
                PageType.Announcement when route.Announcement is not null =>
                    await RenderAnnouncementDetailAsync(route),
                PageType.StaticPage => RenderStaticPage(route),
                _ => NotFound()
            };
        }

        // ============================================================
        // HOME
        // ============================================================

        /// <summary>
        /// Ana sayfa: menüler + en güncel bannerlar, duyurular ve haberler.
        /// </summary>
        private async Task<IActionResult> RenderHomeAsync(
            RouteResolveResult route)
        {
            var siteId = route.Page.SiteId;
            var languageId = route.LanguageId;

            var siteTask = _siteService.GetSiteByIdAsync(siteId);
            var menusTask = _menuService.GetMenusAsync(siteId, languageId);
            var bannersTask = _bannerService.GetBannersAsync(siteId, languageId);
            var haberlerTask = _haberService.GetHabersAsync(siteId, languageId);
            var duyurularTask = _duyuruService.GetDuyurularAsync(siteId, languageId);

            await Task.WhenAll(siteTask, menusTask, bannersTask, haberlerTask, duyurularTask);

            var siteResult = await siteTask;

            if (!siteResult.IsSuccess || siteResult.Data is null)
            {
                _logger.LogWarning("Home sayfası için site bulunamadı. SiteId: {SiteId}", siteId);
                return NotFound();
            }

            var menusResult = await menusTask;
            var bannersResult = await bannersTask;
            var haberlerResult = await haberlerTask;
            var duyurularResult = await duyurularTask;

            var model = new TemplatePageViewModel {
                Site = siteResult.Data,
                Menus = menusResult.Data ?? [],
                Banners = (bannersResult.Data ?? [])
                    .OrderByDescending(b => b.YayimTarihi)
                    .Take(HomeLatestContentCount)
                    .ToList(),
                Haberler = (haberlerResult.Data ?? [])
                    .OrderByDescending(h => h.YayimTarihi)
                    .Take(HomeLatestContentCount)
                    .ToList(),
                Duyurular = (duyurularResult.Data ?? [])
                    .OrderByDescending(d => d.YayimTarihi)
                    .Take(HomeLatestContentCount)
                    .ToList()
            };

            var viewPath = GetTemplateViewPath(route.Page.TemplateId, "Index");

            return View(viewPath, model);
        }

        // ============================================================
        // HABERLER
        // ============================================================

        /// <summary>
        /// /haberler
        /// </summary>
        private async Task<IActionResult> RenderNewListAsync(
            RouteResolveResult route)
        {
            var model = await _haberService.GetHabersAsync(
                route.Page.SiteId, 1);

            if (!model.IsSuccess || model.Data is null)
                return NotFound();

            var viewPath = GetTemplateViewPath(
                route.Page.TemplateId,
                "NewList");

            return View(viewPath, model.Data);
        }

        /// <summary>
        /// /haberler/haber-slug
        /// </summary>
        private async Task<IActionResult> RenderNewDetailAsync(
            RouteResolveResult route)
        {
            if (route.New is null)
            {
                return NotFound();
            }

            var model = await _haberService.GetHaberByIdAsync(
                route.New.Id);

            if (!model.IsSuccess || model.Data is null)
            {
                return NotFound();
            }

            var viewPath = GetTemplateViewPath(route.Page.TemplateId,"New");

            return View(viewPath, model.Data);
        }

        // ============================================================
        // DUYURULAR
        // ============================================================

        /// <summary>
        /// /duyurular
        /// </summary>
        private async Task<IActionResult> RenderAnnouncementListAsync(
            RouteResolveResult route)
        {
            var model = await _duyuruService.GetDuyurularAsync(
                route.Page.SiteId, 1);

            if (!model.IsSuccess || model.Data is null)
                return NotFound();

            var viewPath = GetTemplateViewPath(
                route.Page.TemplateId,
                "AnnouncementList");

            return View(viewPath, model.Data);
        }

        /// <summary>
        /// /duyurular/duyuru-slug
        /// </summary>
        private async Task<IActionResult> RenderAnnouncementDetailAsync(
            RouteResolveResult route)
        {
            if (route.Announcement is null)
            {
                return NotFound();
            }

            var model = await _duyuruService.GetDuyuruByIdAsync(
                route.Announcement.Id);

            if (!model.IsSuccess || model.Data is null)
            {
                return NotFound();
            }

            var viewPath = GetTemplateViewPath(
                route.Page.TemplateId,
                "Announcement");

            return View(viewPath, model.Data);
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
        private IActionResult RenderStaticPage(
            RouteResolveResult route)
        {
            var viewName = route.Page.ViewName;

            if (string.IsNullOrWhiteSpace(viewName))
            {
                _logger.LogError(
                    "Static sayfada ViewName tanımlı değil. PageId: {PageId}",
                    route.Page.Id);

                return NotFound();
            }

            var viewPath = GetTemplateViewPath(
                route.Page.TemplateId,
                viewName);

            return View(viewPath);
        }

        // ============================================================
        // TEMPLATE VIEW PATH
        // ============================================================

        private static string GetTemplateViewPath(
            int templateId,
            string viewName)
        {
            if (templateId <= 0)
            {
                templateId = 1;
            }

            return $"~/Views/Templates/Template{templateId}/{viewName}.cshtml";
        }
    }
}