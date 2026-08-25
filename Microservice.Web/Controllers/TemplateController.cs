using Microservice.Web.Services;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Settings;
using Microservice.Web.ViewModels.PageRoute;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Web.Controllers
{
    public class TemplateController : Controller
    {
        private readonly IRouteService _routeService;
        private readonly IHaberService _haberService;
        private readonly IDuyuruService _duyuruService;
        private readonly ILogger<TemplateController> _logger;

        public TemplateController(
            IRouteService routeService,
            IHaberService haberService,
            IDuyuruService duyuruService,
            ILogger<TemplateController> logger)
        {
            _routeService = routeService;
            _haberService = haberService;
            _duyuruService = duyuruService;
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
            var host = Request.Host.Host;
            var path = Request.Path.Value ?? "/";

            // "/" ise varsayılan dile yönlendir
            if (path == "/" || string.IsNullOrWhiteSpace(path))
            {
                var defaultLanguageCode = "tr";

                return RedirectPermanent($"/{defaultLanguageCode}");
            }

            var route = await _routeService.ResolveAsync(
                host,
                path);

            if (route is null)
            {
                return NotFound();
            }

            return await RenderPageAsync(route);
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

            return (PageType)page.PageTypeId switch {
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