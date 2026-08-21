using Microservice.Web.Services.Interfaces;
using Microservice.Web.Settings;
using Microservice.Web.ViewModels.Duyuru;
using Microservice.Web.ViewModels.Haber;
using Microservice.Web.ViewModels.Menu;
using Microservice.Web.ViewModels.PageRoute;
using Microservice.Web.ViewModels.Template;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Web.Controllers
{
    public class TemplateController : Controller
    {
        private const int DefaultDilId = 1;
        private readonly IPageRouteService _pageRouteService;
        private readonly ISiteService _siteService;
        private readonly IMenuService _menuService;
        private readonly IHaberService _haberService;
        private readonly IDuyuruService _duyuruService;
        private readonly ILogger<TemplateController> _logger;

        public TemplateController(
            IPageRouteService pageRouteService,
            ISiteService siteService,
            IMenuService menuService,
            IHaberService haberService,
            IDuyuruService duyuruService,
            ILogger<TemplateController> logger)
        {
            _pageRouteService = pageRouteService;
            _siteService = siteService;
            _menuService = menuService;
            _haberService = haberService;
            _duyuruService = duyuruService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var context = await ResolveContextAsync();

            if (context is null)
            {
                return NotFound("Host için site bulunamadı.");
            }

            return View(
                GetTemplateViewPath(
                    context.Site.TemplateId,
                    "Index"),
                context);
        }

        public async Task<IActionResult> Seo(string? seoUrl)
        {
            if (string.IsNullOrWhiteSpace(seoUrl))
            {
                return await Index();
            }

            var context = await ResolveContextAsync();

            if (context is null)
            {
                return NotFound("Host için site bulunamadı.");
            }

            var slug = NormalizeSeoValue(seoUrl);

            var route = await _pageRouteService.GetPageRouteBySlugAsync(context.Site.Id, slug);

            if (route.Data is null)
            {
                _logger.LogWarning(
                    "Route bulunamadı. Host: {Host}, Slug: {Slug}",
                    Request.Host.Host,
                    slug);

                return NotFound();
            }

            return route.Data.PageTypeId switch {
                (int)PageTypeEnum.Menu =>
                    await RenderMenu(context, route.Data),

                (int)PageTypeEnum.NewsList =>
                    await RenderNewsList(context, route.Data),

                (int)PageTypeEnum.News =>
                    await RenderNews(context, route.Data),

                (int)PageTypeEnum.Announcement =>
                    await RenderAnnouncement(context, route.Data),

                _ => NotFound()
            };
        }
        private async Task<IActionResult> RenderNews(
    TemplatePageViewModel context,
    PageRouteSlugDetailVm route)
        {
            if (!route.ContentId.HasValue)
            {
                return NotFound();
            }

            var haber = await _haberService.GetHaberByIdAsync(route.ContentId.Value);

            if (haber.Data is null)
            {
                return NotFound();
            }

            var model = new HaberPageViewModel {
                Site = context.Site,
                Menus = context.Menus,
                CurrentMenu = context.CurrentMenu,
                Haber = haber.Data,
                Seo = route.SeoMetadata
            };

            return View(
                GetTemplateViewPath(
                    context.Site.TemplateId,
                    "News"),
                model);
        }
        private async Task<IActionResult> RenderNewsList(
    TemplatePageViewModel context,
    PageRouteSlugDetailVm route)
        {
            var news = await _haberService.GetHabersAsync(context.Site.Id, DefaultDilId);

            if (news.Data is null)
            {
                return NotFound();
            }
            var model = new HaberlerPageViewModel {
                Site = context.Site,
                Menus = context.Menus,
                Haberler = news.Data,
                Seo = route.SeoMetadata
            };

            return View(
                GetTemplateViewPath(
                    context.Site.TemplateId,
                    "NewsList"),
                model);
        }
        private async Task<IActionResult> RenderAnnouncement(
     TemplatePageViewModel context,
     PageRouteSlugDetailVm route)
        {
            if (!route.ContentId.HasValue)
            {
                return NotFound();
            }

            var announcement = await _duyuruService.GetDuyuruByIdAsync(route.ContentId.Value);

            if (announcement.Data is null)
            {
                return NotFound();
            }

            var model = new DuyurularPageViewModel {
                Site = context.Site,
                Menus = context.Menus,
                CurrentMenu = context.CurrentMenu,
                Duyuru = announcement.Data,
                Seo = route.SeoMetadata
            };

            return View(
                GetTemplateViewPath(
                    context.Site.TemplateId,
                    "Announcement"),
                model);
        }
        private async Task<IActionResult> RenderMenu(
    TemplatePageViewModel context,
    PageRouteSlugDetailVm route)
        {
            var currentMenu = Flatten(context.Menus)
                .FirstOrDefault(x =>
                    NormalizeSeoValue(x.Link) ==
                    NormalizeSeoValue(route.Slug));

            context.CurrentMenu = currentMenu;
            context.Seo =route.SeoMetadata;

            return View(
                GetTemplateViewPath(
                    context.Site.TemplateId,
                    "Menu"),
                context);
        }
        private async Task<TemplatePageViewModel?> ResolveContextAsync()
        {
            var host = Request.Host.Host;

            if (string.IsNullOrWhiteSpace(host))
            {
                return null;
            }

            var siteResult = await _siteService.GetSiteByHostAsync(host);

            if (siteResult.IsFail ||
                siteResult.Data is null)
            {
                return null;
            }

            var menuResult =
                await _menuService.GetMenusAsync(
                    siteResult.Data.Id,
                    DefaultDilId);

            var menus =
                menuResult.Data ??
                new List<MenuGetVm>();

            return new TemplatePageViewModel {
                Site = siteResult.Data,
                Menus = menus
            };
        }

        private static string GetTemplateViewPath(
       int templateId,
       string pageName)
        {
            if (templateId <= 0)
            {
                templateId = 1;
            }

            return
                $"~/Views/Templates/Template{templateId}/{pageName}.cshtml";
        }

        private static List<MenuGetVm> Flatten(
         List<MenuGetVm> menus)
        {
            var all = new List<MenuGetVm>();

            foreach (var menu in menus)
            {
                all.Add(menu);

                if (menu.Children.Count > 0)
                {
                    all.AddRange(
                        Flatten(menu.Children));
                }
            }

            return all;
        }

        private static string NormalizeSeoValue(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var candidate = value.Trim();

            if (Uri.TryCreate(
                candidate,
                UriKind.Absolute,
                out var uri))
            {
                candidate = uri.AbsolutePath;
            }

            candidate =
                candidate.Trim('/')
                    .ToLowerInvariant();

            return Uri.UnescapeDataString(candidate);
        }
    }
}