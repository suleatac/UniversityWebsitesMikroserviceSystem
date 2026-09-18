using Microservice.Web.Models;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.Settings;
using Microservice.Web.ViewModels.Bilgi;
using Microservice.Web.ViewModels.Duyuru;
using Microservice.Web.ViewModels.Etkinlik;
using Microservice.Web.ViewModels.GaleriResim;
using Microservice.Web.ViewModels.Haber;
using Microservice.Web.ViewModels.Icerik;
using Microservice.Web.ViewModels.Iletisim;
using Microservice.Web.ViewModels.Menu;
using Microservice.Web.ViewModels.PageRoute;
using Microservice.Web.ViewModels.SitePersonel;
using Microservice.Web.ViewModels.Paged;
using Microservice.Web.ViewModels.Search;
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
        private readonly IIcerikService _icerikService;
        private readonly IPageTypeService _pageTypeService;
        private readonly ISitePersonelService _sitePersonelService;
        private readonly IGaleriResimService _galeriResimService;
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
            IIcerikService icerikService,
            IPageTypeService pageTypeService,
            ISitePersonelService sitePersonelService,
            IGaleriResimService galeriResimService,
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
            _icerikService = icerikService;
            _pageTypeService = pageTypeService;
            _sitePersonelService = sitePersonelService;
            _galeriResimService = galeriResimService;
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

            //var host = "default.sivas.edu.tr";
            //var path = "/tr/";

            ViewData["Host"] = host;
            ViewData["Path"] = path;
        

            var route = await _routeService.ResolveAsync(
                host,
                path);

            if (route is null)
            {
                return RenderNotFound(
                    $"'{path}' adresi ve '{host}' için geçerli bir sayfa bulunamadı.");
     
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

            // Navbar'daki arama formunun gidecegi sayfanin slug'i (PageTypeKind.Search).
            // Site API tarafinda 10 dk cache'lendigi icin her istekte ek maliyet olusturmaz.
            var searchPageType = await _pageTypeService.GetPageTypeByKindAsync(
                route.Site.TemplateId,
                route.LanguageId,
                (int)PageTypeKindEnum.Search);

            ViewData["SearchPageSlug"] = searchPageType.Data?.Slug;
   

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

                PageTypeKindEnum.VideoListesi when route.DetailSlug is null =>
                     await RenderVideoListesiAsync(route),
                PageTypeKindEnum.VideoDetay when route.VideoDetay is not null =>
                     await RenderVideoDetayAsync(route),


                PageTypeKindEnum.PersonelListesi when route.DetailSlug is null =>
                     await RenderPersonelListesiAsync(route),
                PageTypeKindEnum.PersonelDetay when route.PersonelDetay is not null =>
                     await RenderPersonelDetayAsync(route),

                PageTypeKindEnum.Search when route.DetailSlug is null =>
                     await RenderSearchAsync(route),

                PageTypeKindEnum.GaleriResimListesi when route.DetailSlug is null =>
                     await RenderGaleriResimListesiAsync(route),
                PageTypeKindEnum.GaleriResimDetay when route.GaleriResimDetay is not null =>
                     await RenderGaleriResimDetayAsync(route),

                PageTypeKindEnum.StaticPage => await RenderStaticPageAsync(route),
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
            // Sidebar arama widget'i genel arama sayfasina yonlendirir.
            var duyurularPageTypeTask = _pageTypeService.GetPageTypeByKindAsync(
                route.Site.TemplateId,
                route.LanguageId,
                (int)PageTypeKindEnum.DuyuruListesi);
            var haberlerPageTypeTask = _pageTypeService.GetPageTypeByKindAsync(
                route.Site.TemplateId,
                route.LanguageId,
                (int)PageTypeKindEnum.HaberListesi);
            await Task.WhenAll(
                siteTask, 
                menusTask, 
                bannersTask, 
                haberlerTask, 
                duyurularTask, 
                shortcutButtonsTask, 
                bilgiTask, 
                etkinliklerTask,
                duyurularPageTypeTask,
                haberlerPageTypeTask
                );

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
            var duyurularPageTypeResult = await duyurularPageTypeTask;
            var haberlerPageTypeResult = await haberlerPageTypeTask;

            var model = new TemplatePageViewModel {
                Site = siteResult.Data,
                HaberListUrl= $"/{route.LanguageCode}/{haberlerPageTypeResult.Data.Slug}",
                DuyuruListUrl= $"/{route.LanguageCode}/{duyurularPageTypeResult.Data.Slug}",
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

            var menu = route.MenuDetay;

            // Sidebar arama widget'i genel arama sayfasina yonlendirir.
            var searchPageType = await _pageTypeService.GetPageTypeByKindAsync(
                route.Site.TemplateId,
                route.LanguageId,
                (int)PageTypeKindEnum.Search);

            var model = new MenuDetayPageViewModel {
                Menu = menu,
                Site = route.Site,
                LanguageCode = route.LanguageCode,
                SearchUrl = searchPageType.Data is null
                    ? "/"
                    : $"/{route.LanguageCode}/{searchPageType.Data.Slug}"
            };

            // Navbar component'inin site bilgisini tekrar cekmesini engellemek icin paylasilir.
            ViewData["Site"] = route.Site;

            var viewPath = GetTemplateViewPath(route.Page.TemplateId, menu.PageType.ViewName);

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

            var etkinlik = route.EtkinlikDetay;

            // Sidebar "Son Eklenenler" widget'i icin en yeni etkinlikler (mevcut etkinlik haric).
            var etkinliklerResult = await _etkinlikService.GetEtkinliklerAsync(route.Site.Id, route.LanguageId);
            // Liste sayfasi adresi: route.Page, DuyuruDetay sayfa tipidir; liste icin DuyuruListesi cozulmeli.
            var etkinlikListePageType = await _pageTypeService.GetPageTypeByKindAsync(
                route.Site.TemplateId,
                route.LanguageId,
                (int)PageTypeKindEnum.Etkinlik);


            var latestEtkinlikler = (etkinliklerResult.Data ?? [])
                .Where(e => e.Id != etkinlik.Id)
                .OrderByDescending(e => e.YayimTarihi)
                .Take(3)
                .ToList();

            var model = new EtkinlikDetayPageViewModel {
                Etkinlik = etkinlik,
                Site = route.Site,
                LanguageCode = route.LanguageCode,
                // route.Page, /{dil}/{etkinlik-slug}/{seoUrl} icindeki liste sayfasi
                EtkinlikListUrl = $"/{route.LanguageCode}/{etkinlikListePageType.Data.Slug}",
                LatestEtkinlikler = latestEtkinlikler
            };

            // Navbar component'inin site bilgisini tekrar cekmesini engellemek icin paylasilir.
            ViewData["Site"] = route.Site;

            var viewPath = GetTemplateViewPath(route.Page.TemplateId, etkinlik.PageType.ViewName);

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

            var bilgi = route.BilgiDetay;

            // Sidebar "Son Eklenenler" widget'i icin en yeni bilgiler (mevcut bilgi haric).
            var bilgilerResult = await _bilgiService.GetBilgisAsync(route.Site.Id, route.LanguageId);
            // Liste sayfasi adresi: route.Page, DuyuruDetay sayfa tipidir; liste icin DuyuruListesi cozulmeli.
            var bilgiListePageType = await _pageTypeService.GetPageTypeByKindAsync(
                route.Site.TemplateId,
                route.LanguageId,
                (int)PageTypeKindEnum.Etkinlik);

            var latestBilgiler = (bilgilerResult.Data ?? [])
                .Where(b => b.Id != bilgi.Id)
                .OrderByDescending(b => b.YayimTarihi)
                .Take(3)
                .ToList();

            var model = new BilgiDetayPageViewModel {
                Bilgi = bilgi,
                Site = route.Site,
                LanguageCode = route.LanguageCode,
                // route.Page, /{dil}/{bilgi-slug}/{seoUrl} icindeki liste sayfasi
                BilgiListUrl = $"/{route.LanguageCode}/{bilgiListePageType.Data.Slug}",
                LatestBilgiler = latestBilgiler
            };

            // Navbar component'inin site bilgisini tekrar cekmesini engellemek icin paylasilir.
            ViewData["Site"] = route.Site;

            var viewPath = GetTemplateViewPath(route.Page.TemplateId, bilgi.PageType.ViewName);

            return View(viewPath, model);
        }



        // ============================================================
        // PERSONELLER
        // ============================================================

        /// <summary>
        /// /personeller?q=kelime
        /// A-Z harf filtreli (tema isotope) ve sayfa ici arama destekli personel listesi.
        /// </summary>
        private async Task<IActionResult> RenderPersonelListesiAsync(RouteResolveResult route)
        {
            // Route resolver personel listesini zaten getirdi; yoksa servis uzerinden cekilir.
            var personeller = route.PersonelListesi;

            if (personeller is null)
            {
                var result = await _sitePersonelService.GetPersonelListAsync(route.Site.Id);
                personeller = result.Data ?? [];
            }

            var model = new PersonelListPageViewModel {
                Site = route.Site,
                LanguageCode = route.LanguageCode,
                Query = Request.Query["q"].ToString(),
                // Once unvanin sira numarasi (kucuk = ust unvan), sonra soyad/ad alfabetik.
                Personeller = personeller
                    .OrderBy(p => p.UnvanSira)
                    .ThenBy(p => p.Soyadi)
                    .ThenBy(p => p.Adi)
                    .ToList()
            };

            // Navbar component'inin site bilgisini tekrar cekmesini engellemek icin paylasilir.
            ViewData["Site"] = route.Site;

            var viewPath = GetTemplateViewPath(route.Page.TemplateId, "PersonelListesi");

            return View(viewPath, model);
        }

        /// <summary>
        /// /personeller/personel-slug
        /// </summary>
        private async Task<IActionResult> RenderPersonelDetayAsync(RouteResolveResult route)
        {
            if (route.PersonelDetay is null)
            {
                return RenderNotFound("Personel bulunamadı.");
            }

            var personel = route.PersonelDetay;

            // Sidebar "Son Eklenenler" widget'i icin personel listesi (mevcut personel haric).
            var personelListesiResult = await _sitePersonelService.GetPersonelListAsync(route.Site.Id);

            var latestPersoneller = (personelListesiResult.Data ?? [])
                .Where(p => p.Id != personel.Id)
                .OrderBy(p => p.UnvanSira)
                .ThenBy(p => p.Soyadi)
                .ThenBy(p => p.Adi)
                .Take(3)
                .ToList();

            // Liste sayfasi adresi: route.Page PersonelDetay tipidir; liste icin PersonelListesi cozulmeli.
            var personelListePageType = await _pageTypeService.GetPageTypeByKindAsync(
                route.Site.TemplateId,
                route.LanguageId,
                (int)PageTypeKindEnum.PersonelListesi);

            var model = new PersonelDetayPageViewModel {
                Personel = personel,
                Site = route.Site,
                LanguageCode = route.LanguageCode,
                PersonelListUrl = personelListePageType.Data is null
                    ? "/"
                    : $"/{route.LanguageCode}/{personelListePageType.Data.Slug}",
                LatestPersoneller = latestPersoneller
            };

            // Navbar component'inin site bilgisini tekrar cekmesini engellemek icin paylasilir.
            ViewData["Site"] = route.Site;

            var viewPath = GetTemplateViewPath(route.Page.TemplateId, personel.PageType.ViewName);

            return View(viewPath, model);
        }














        // ============================================================
        // HABERLER
        // ============================================================

        /// <summary>
        /// /haberler?q=kelime&page=1&pageSize=5
        /// Sayfali ve sayfa ici arama destekli haber listesi.
        /// </summary>
        private async Task<IActionResult> RenderHaberListesiAsync(RouteResolveResult route)
        {
            var model = await BuildContentListAsync<GetHaberVm>(
                (q, page, pageSize) => _haberService.GetPaginatedAsync(
                    route.Site.Id, route.LanguageId, q, page, pageSize),
                route);

            var viewPath = GetTemplateViewPath(route.Page.TemplateId, "HaberListesi");

            return View(viewPath, model);
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

            var haber = route.HaberDetay;

            // Sidebar "Son Eklenenler" widget'i icin en yeni haberler (mevcut haber haric).
            var habersResult = await _haberService.GetHabersAsync(route.Site.Id, route.LanguageId);
            // Sidebar "Son Eklenenler" widget'i icin en yeni haberler (mevcut haber haric).
            var pageTypeResult = await _pageTypeService.GetPageTypeByKindAsync(route.Site.TemplateId,route.LanguageId, (int)PageTypeKindEnum.HaberListesi);

            var latestHabers = (habersResult.Data ?? [])
                .Where(h => h.Id != haber.Id)
                .OrderByDescending(h => h.YayimTarihi)
                .Take(3)
                .ToList();

            var model = new HaberDetayPageViewModel {
                Haber = haber,
                Site = route.Site,
                LanguageCode = route.LanguageCode,
                // route.Page, /{dil}/{haberler-slug}/{seoUrl} icindeki liste sayfasi
                HaberListUrl = $"/{route.LanguageCode}/{pageTypeResult.Data.Slug}",
                LatestHabers = latestHabers
            };

            // Navbar component'inin site bilgisini tekrar cekmesini engellemek icin paylasilir.
            ViewData["Site"] = route.Site;

            var viewPath = GetTemplateViewPath(route.Page.TemplateId, haber.PageType.ViewName);

            return View(viewPath, model);
        }




        // ============================================================
        // VİDEOLAR
        // ============================================================

        /// <summary>
        /// /videolar
        /// </summary>
        private async Task<IActionResult> RenderVideoListesiAsync(RouteResolveResult route)
        {
            if (route.DuyuruListesi is null)
            {
                return RenderNotFound("Duyurular bulunamadı.");
            }

            var viewPath = GetTemplateViewPath(route.Page.TemplateId, "DuyuruListesi");

            return View(viewPath, route.DuyuruListesi);
        }

        /// <summary>
        /// /videolar/video-slug
        /// </summary>
        private async Task<IActionResult> RenderVideoDetayAsync(RouteResolveResult route)
        {
            if (route.VideoDetay is null)
            {
                return RenderNotFound("Video bulunamadı.");
            }

            var model = route.VideoDetay;



            var viewPath = GetTemplateViewPath(route.Page.TemplateId, model.PageType.ViewName);

            return View(viewPath, model);
        }












        // ============================================================
        // GALERİ RESİMLERİ
        // ============================================================

        /// <summary>
        /// /galeri-resimler?q=kelime&kategori=x&page=1&pageSize=8
        /// Sayfali; kategori ve sayfa ici arama destekli galeri resmi listesi.
        /// </summary>
        private async Task<IActionResult> RenderGaleriResimListesiAsync(RouteResolveResult route)
        {
            var query = Request.Query["q"].ToString();
            var kategori = Request.Query["kategori"].ToString();

            int.TryParse(Request.Query["page"], out var pageParam);
            int.TryParse(Request.Query["pageSize"], out var pageSizeParam);

            var page = pageParam < 1 ? 1 : pageParam;
            // Galeri grid'i icin varsayilan 8; 100 ust sinir.
            var pageSize = pageSizeParam is > 0 and <= 100 ? pageSizeParam : 8;

            var listResult = await _galeriResimService.GetPaginatedAsync(
                route.Site.Id,
                route.LanguageId,
                string.IsNullOrWhiteSpace(query) ? null : query,
                string.IsNullOrWhiteSpace(kategori) ? null : kategori,
                page,
                pageSize);

            if (listResult.IsFail)
            {
                _logger.LogWarning(
                    "Galeri resmi liste verileri alinamadi. SiteId: {SiteId}, Query: {Query}, Kategori: {Kategori}",
                    route.Site.Id,
                    query,
                    kategori);
            }

            // Filtre cabugu icin benzersiz kategoriler (site geneli, API tarafinda 12 saat cache'li).
            var allResult = await _galeriResimService.GetGaleriResimlerAsync(route.Site.Id, route.LanguageId);

            var kategoriler = (allResult.Data ?? [])
                .Where(g => !string.IsNullOrWhiteSpace(g.Kategori))
                .Select(g => g.Kategori!)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(k => k)
                .ToList();

            var model = new GaleriResimListPageViewModel {
                Site = route.Site,
                LanguageCode = route.LanguageCode,
                Query = query,
                Kategori = kategori,
                Page = page,
                PageSize = pageSize,
                Results = listResult.Data ?? new PagedResultVm<GetGaleriResimVm>(),
                Kategoriler = kategoriler
            };

            // Navbar component'inin site bilgisini tekrar cekmesini engellemek icin paylasilir.
            ViewData["Site"] = route.Site;

            var viewPath = GetTemplateViewPath(route.Page.TemplateId, "GaleriResimListesi");

            return View(viewPath, model);
        }

        /// <summary>
        /// /galeri-resimler/resim-slug
        /// Detay + onceki/sonraki navigasyonu + iliskili resimler.
        /// </summary>
        private async Task<IActionResult> RenderGaleriResimDetayAsync(RouteResolveResult route)
        {
            if (route.GaleriResimDetay is null)
            {
                return RenderNotFound("Galeri resmi bulunamadı.");
            }

            var galeriResim = route.GaleriResimDetay;

            // Navigasyon ve iliskili resimler icin sitenin tam resmi listesi.
            var galeriResimleriResult = await _galeriResimService.GetGaleriResimlerAsync(route.Site.Id, route.LanguageId);

            // Liste sayfasi adresi: route.Page, GaleriResimDetay sayfa tipidir; liste icin GaleriResimListesi cozulmeli.
            var galeriListePageType = await _pageTypeService.GetPageTypeByKindAsync(
                route.Site.TemplateId,
                route.LanguageId,
                (int)PageTypeKindEnum.GaleriResimListesi);

            var galeriListUrl = galeriListePageType.Data is null
                ? "/"
                : $"/{route.LanguageCode}/{galeriListePageType.Data.Slug}";

            var sirali = (galeriResimleriResult.Data ?? [])
                .OrderBy(g => g.Sira)
                .ThenByDescending(g => g.YayimTarihi)
                .ToList();

            var currentIndex = sirali.FindIndex(g => g.Id == galeriResim.Id);

            // Ayni kategoriden (yetersizse listeden) en fazla 4 iliskili resim.
            var iliskili = sirali
                .Where(g => g.Id != galeriResim.Id)
                .OrderByDescending(g => string.Equals(g.Kategori, galeriResim.Kategori, StringComparison.OrdinalIgnoreCase))
                .Take(4)
                .ToList();

            var model = new GaleriResimDetayPageViewModel {
                GaleriResim = galeriResim,
                Site = route.Site,
                LanguageCode = route.LanguageCode,
                GaleriResimListUrl = galeriListUrl,
                OncekiResim = currentIndex > 0 ? sirali[currentIndex - 1] : null,
                SonrakiResim = currentIndex >= 0 && currentIndex < sirali.Count - 1 ? sirali[currentIndex + 1] : null,
                IliskiliResimler = iliskili
            };

            // Navbar component'inin site bilgisini tekrar cekmesini engellemek icin paylasilir.
            ViewData["Site"] = route.Site;

            var viewPath = GetTemplateViewPath(route.Page.TemplateId, galeriResim.PageType.ViewName);

            return View(viewPath, model);
        }

        // ============================================================
        // DUYURULAR
        // ============================================================

        /// <summary>
        /// /duyurular?q=kelime&page=1&pageSize=5
        /// Sayfali ve sayfa ici arama destekli duyuru listesi.
        /// </summary>
        private async Task<IActionResult> RenderDuyuruListesiAsync(RouteResolveResult route)
        {
            var model = await BuildContentListAsync<GetDuyuruVm>(
                (q, page, pageSize) => _duyuruService.GetPaginatedAsync(
                    route.Site.Id, route.LanguageId, q, page, pageSize),
                route);

            var viewPath = GetTemplateViewPath(route.Page.TemplateId, "DuyuruListesi");

            return View(viewPath, model);
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

            var duyuru = route.DuyuruDetay;

            // Sidebar "Son Eklenenler" widget'i icin en yeni duyurular (mevcut duyuru haric).
            var duyurularResult = await _duyuruService.GetDuyurularAsync(route.Site.Id, route.LanguageId);

            // Liste sayfasi adresi: route.Page, DuyuruDetay sayfa tipidir; liste icin DuyuruListesi cozulmeli.
            var duyuruListePageType = await _pageTypeService.GetPageTypeByKindAsync(
                route.Site.TemplateId,
                route.LanguageId,
                (int)PageTypeKindEnum.DuyuruListesi);

            var latestDuyurular = (duyurularResult.Data ?? [])
                .Where(d => d.Id != duyuru.Id)
                .OrderByDescending(d => d.YayimTarihi)
                .Take(3)
                .ToList();

            var model = new DuyuruDetayPageViewModel {
                Duyuru = duyuru,
                Site = route.Site,
                LanguageCode = route.LanguageCode,
                DuyuruListUrl = duyuruListePageType.Data is null
                    ? "/"
                    : $"/{route.LanguageCode}/{duyuruListePageType.Data.Slug}",
                LatestDuyurular = latestDuyurular
            };

            // Navbar component'inin site bilgisini tekrar cekmesini engellemek icin paylasilir.
            ViewData["Site"] = route.Site;

            var viewPath = GetTemplateViewPath(route.Page.TemplateId, duyuru.PageType.ViewName);

            return View(viewPath, model);
        }

        // ============================================================
        // ARAMA
        // ============================================================

        /// <summary>
        /// /arama?q=kelime&tip=1&page=1
        /// Icerik (Haber, Duyuru, Bilgi, Etkinlik, Video) tabaninda sayfali arama.
        /// </summary>
        private async Task<IActionResult> RenderSearchAsync(RouteResolveResult route)
        {
            var siteId = route.Site.Id;
            var languageId = route.LanguageId;

            var query = Request.Query["q"].ToString();

            int.TryParse(Request.Query["tip"], out var tipParam);
            int.TryParse(Request.Query["page"], out var pageParam);
            int.TryParse(Request.Query["pageSize"], out var pageSizeParam);

            var page = pageParam < 1 ? 1 : pageParam;
            var pageSize = pageSizeParam is > 0 and <= 100 ? pageSizeParam : 5;
            int? tip = tipParam is >= 1 and <= 5 ? tipParam : null;

            var searchResult = await _icerikService.SearchAsync(
                siteId,
                languageId,
                query,
                tip,
                page,
                pageSize);

            if (searchResult.IsFail && !string.IsNullOrWhiteSpace(query))
            {
                _logger.LogWarning(
                    "Arama sonuclari alinamadi. SiteId: {SiteId}, Query: {Query}",
                    siteId,
                    query);
            }

            var model = new SearchPageViewModel {
                Site = route.Site,
                LanguageCode = route.LanguageCode,
                Query = query,
                Tip = tip,
                Page = page,
                Results = searchResult.Data ?? new PagedResultVm<IcerikSearchVm>()
            };

            // Navbar component'inin site bilgisini tekrar cekmesini engellemek icin paylasilir.
            ViewData["Site"] = route.Site;

            var viewPath = GetTemplateViewPath(route.Page.TemplateId, "Search");

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
        private async Task<IActionResult> RenderStaticPageAsync(RouteResolveResult route)
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

            // Iletisim sayfasi form + site iletisim bilgileri gerektirir; model ile render edilir.
            if (string.Equals(viewName, "Iletisim", StringComparison.OrdinalIgnoreCase))
            {
                var habersResult = await _haberService.GetHabersAsync(route.Site.Id, route.LanguageId);

                var latestHabers = (habersResult.Data ?? [])
                    .OrderByDescending(h => h.YayimTarihi)
                    .Take(3)
                    .ToList();

                // Navbar component'inin site bilgisini tekrar cekmesini engellemek icin paylasilir.
                ViewData["Site"] = route.Site;

                var model = new IletisimPageViewModel {
                    Site = route.Site,
                    LanguageCode = route.LanguageCode,
                    Form = new IletisimMesajiVm { SiteId = route.Site.Id },
                    SearchUrl = ViewData["SearchPageSlug"] is string searchSlug && !string.IsNullOrWhiteSpace(searchSlug)
                        ? $"/{route.LanguageCode}/{searchSlug}"
                        : "/",
                    LatestHabers = latestHabers
                };

                return View(viewPath, model);
            }

            return View(viewPath);
        }

        // ============================================================
        // LISTE SAYFALARI ICIN ORTAK SAYFALAMA/ARAMA KURUCU
        // ============================================================

        /// <summary>
        /// ?q / ?page / ?pageSize query parametrelerini okuyup ilgili servisten
        /// sayfali listeyi ceken ortak yardimci (HaberListesi, DuyuruListesi vb.).
        /// </summary>
        private async Task<ContentListPageViewModel<T>> BuildContentListAsync<T>(
            Func<string?, int, int, Task<ServiceResult<PagedResultVm<T>>>> fetch,
            RouteResolveResult route)
        {
            var query = Request.Query["q"].ToString();

            int.TryParse(Request.Query["page"], out var pageParam);
            int.TryParse(Request.Query["pageSize"], out var pageSizeParam);

            var page = pageParam < 1 ? 1 : pageParam;
            var pageSize = pageSizeParam is > 0 and <= 100 ? pageSizeParam : 5;

            var listResult = await fetch(string.IsNullOrWhiteSpace(query) ? null : query, page, pageSize);

            if (listResult.IsFail)
            {
                _logger.LogWarning(
                    "Liste sayfasi verileri alinamadi. SiteId: {SiteId}, Query: {Query}",
                    route.Site.Id,
                    query);
            }

            // Navbar component'inin site bilgisini tekrar cekmesini engellemek icin paylasilir.
            ViewData["Site"] = route.Site;

            return new ContentListPageViewModel<T> {
                Site = route.Site,
                LanguageCode = route.LanguageCode,
                Query = query,
                Page = page,
                Results = listResult.Data ?? new PagedResultVm<T>()
            };
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