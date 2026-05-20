using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IYoneticiSiteService _yoneticiSiteService;
        private readonly IHaberService _haberService;
        private readonly IDuyuruService _duyuruService;
        private readonly IEtkinlikService _etkinlikService;
        private readonly IYonetimDuyuruService _yonetimDuyuruService;
        private readonly IAuditLogService _auditLogService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ICurrentUserService currentUserService,
            IYoneticiSiteService yoneticiSiteService,
            IHaberService haberService,
            IDuyuruService duyuruService,
            IEtkinlikService etkinlikService,
            IYonetimDuyuruService yonetimDuyuruService,
            IAuditLogService auditLogService,
            ILogger<HomeController> logger)
        {
            _currentUserService = currentUserService;
            _yoneticiSiteService = yoneticiSiteService;
            _haberService = haberService;
            _duyuruService = duyuruService;
            _etkinlikService = etkinlikService;
            _yonetimDuyuruService = yonetimDuyuruService;
            _auditLogService = auditLogService;
            _logger = logger;
        }

        // GET: Home
        public async Task<IActionResult> Index()
        {
            try
            {
                // Admin rolüne sahipse site seçimi zorunlu
                ViewBag.IsCurrentUserAdmin = _currentUserService.IsAdmin;

                if (_currentUserService.IsAdmin)
                {
                    // Admin henüz site seçmemişse site seçim sayfasına yönlendir
                    var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId");
                    if (!currentSiteId.HasValue || currentSiteId.Value <= 0)
                    {
                        _logger.LogInformation("Admin kullanıcı site seçimi yapmadan anasayfaya erişmeye çalıştı. Site seçim sayfasına yönlendiriliyor.");
                        return RedirectToAction("SelectSite", "SiteSelection");
                    }

                    if (!HttpContext.Session.GetInt32("CurrentDilId").HasValue)
                    {
                        HttpContext.Session.SetInt32("CurrentDilId", 1);
                    }

                    var model = await BuildDashboardViewModelAsync(currentSiteId.Value, HttpContext.Session.GetInt32("CurrentDilId") ?? 1);
                    return View(model);
                }

                // Admin değilse, yetkili olduğu siteleri kontrol et
                var keycloakUserId = _currentUserService.KeycloakUserId;
                var authorizedSitesResult = await _yoneticiSiteService.GetYoneticiSitesByKeycloakUserIdAsync(keycloakUserId);

                if (!authorizedSitesResult.IsSuccess || authorizedSitesResult.Data == null || !authorizedSitesResult.Data.Any())
                {
                    // Yetkili site yoksa uyarı ver ve logout yap
                    _logger.LogWarning("Admin olmayan kullanıcı yetkili siteye sahip değil. KeycloakUserId: {KeycloakUserId}", keycloakUserId);
                    TempData["Warning"] = "Yetkili olduğunuz bir site bulunmamaktadır. Lütfen yöneticinizle iletişime geçiniz.";
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return RedirectToAction("SignIn", "Auth");
                }

                // Yetkili siteleri session'a kaydet
                var authorizedSiteIds = authorizedSitesResult.Data.Select(s => s.SiteId).ToList();
                HttpContext.Session.SetString("AuthorizedSiteIds", string.Join(",", authorizedSiteIds));

                // Session'da site seçimi var mı kontrol et
                var existingSiteId = HttpContext.Session.GetInt32("CurrentSiteId");

                if (!existingSiteId.HasValue || existingSiteId.Value <= 0)
                {
                    // Sadece 1 yetkili site varsa otomatik seç
                    if (authorizedSitesResult.Data.Count == 1)
                    {
                        var singleSite = authorizedSitesResult.Data.First();
                        HttpContext.Session.SetInt32("CurrentSiteId", singleSite.SiteId);
                        HttpContext.Session.SetInt32("CurrentDilId", 1);
                        _logger.LogInformation("Kullanıcıya tek yetkili site otomatik atandı. SiteId: {SiteId}, KeycloakUserId: {KeycloakUserId}", singleSite.SiteId, keycloakUserId);

                        var dashboardModel = await BuildDashboardViewModelAsync(singleSite.SiteId, 1);
                        return View(dashboardModel);
                    }
                    else
                    {
                        // Birden fazla yetkili site varsa site seçim sayfasına yönlendir
                        _logger.LogInformation("Admin olmayan kullanıcının birden fazla yetkili sitesi var. Site seçim sayfasına yönlendiriliyor. KeycloakUserId: {KeycloakUserId}", keycloakUserId);
                        return RedirectToAction("SelectSite", "SiteSelection");
                    }
                }

                if (!HttpContext.Session.GetInt32("CurrentDilId").HasValue)
                {
                    HttpContext.Session.SetInt32("CurrentDilId", 1);
                }

                var modelForExistingSite = await BuildDashboardViewModelAsync(
                    existingSiteId.Value,
                    HttpContext.Session.GetInt32("CurrentDilId") ?? 1);
                return View(modelForExistingSite);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Home/Index hatası");
                return RedirectToAction("SignIn", "Auth");
            }
        }

        private async Task<DashboardViewModel> BuildDashboardViewModelAsync(int siteId, int dilId)
        {
            var model = new DashboardViewModel();

            try
            {
                // Toplam haber sayısı
                var haberResult = await _haberService.GetHabersAsync(siteId, dilId);
                if (haberResult.IsSuccess && haberResult.Data != null)
                {
                    model.HaberCount = haberResult.Data.Count;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Haber sayısı alınırken hata oluştu. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);
            }

            try
            {
                // Toplam duyuru sayısı
                var duyuruResult = await _duyuruService.GetDuyurularAsync(siteId, dilId);
                if (duyuruResult.IsSuccess && duyuruResult.Data != null)
                {
                    model.DuyuruCount = duyuruResult.Data.Count;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Duyuru sayısı alınırken hata oluştu. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);
            }

            try
            {
                // Toplam etkinlik sayısı
                var etkinlikResult = await _etkinlikService.GetEtkinliklerAsync(siteId, dilId);
                if (etkinlikResult.IsSuccess && etkinlikResult.Data != null)
                {
                    model.EtkinlikCount = etkinlikResult.Data.Count;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Etkinlik sayısı alınırken hata oluştu. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);
            }

            try
            {
                // Son 5 yönetim duyurusu
                var yonetimDuyuruResult = await _yonetimDuyuruService.GetYonetimDuyurusAsync();
                if (yonetimDuyuruResult.IsSuccess && yonetimDuyuruResult.Data != null)
                {
                    model.RecentYonetimDuyurular = yonetimDuyuruResult.Data
                        .OrderByDescending(d => d.Id)
                        .Take(5)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yönetim duyuruları alınırken hata oluştu.");
            }

            try
            {
                // Okunmamış yönetim duyurusu sayısı
                var unreadCountResult = await _yonetimDuyuruService.GetUnreadYonetimDuyuruCountAsync();
                if (unreadCountResult.IsSuccess)
                {
                    model.UnreadYonetimDuyuruCount = unreadCountResult.Data;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Okunmamış yönetim duyurusu sayısı alınırken hata oluştu.");
            }

            try
            {
                // Günlük audit log istatistikleri (bu haftanın günleri)
                var today = DateTime.Today;
                var diff = (7 + (int)today.DayOfWeek - (int)DayOfWeek.Monday) % 7;
                var startOfWeek = today.AddDays(-diff);
                var endOfWeek = startOfWeek.AddDays(7);

                var dailyStatsResult = await _auditLogService.GetAuditLogDailyStatsAsync(startOfWeek, endOfWeek);
                if (dailyStatsResult.IsSuccess && dailyStatsResult.Data != null)
                {
                    model.DailyAuditLogStats = dailyStatsResult.Data;
                    model.TotalAuditLogCount = dailyStatsResult.Data.Sum(x => x.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Audit log günlük istatistikler alınırken hata oluştu.");
            }

            try
            {
                // Son 10 audit log aktivitesi
                var recentLogsResult = await _auditLogService.GetAuditLoglarPaginatedAsync(siteId, dilId, 1, 10, null, "Id", "desc");
                if (recentLogsResult.IsSuccess && recentLogsResult.Data != null && recentLogsResult.Data.Data != null)
                {
                    model.RecentAuditLogs = recentLogsResult.Data.Data.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Son aktiviteler alınırken hata oluştu.");
            }

            return model;
        }
    }
}
