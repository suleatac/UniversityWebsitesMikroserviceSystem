using Microservice.Admin.Services.Interfaces;
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
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ICurrentUserService currentUserService,
            IYoneticiSiteService yoneticiSiteService,
            ILogger<HomeController> logger)
        {
            _currentUserService = currentUserService;
            _yoneticiSiteService = yoneticiSiteService;
            _logger = logger;
        }

        // GET: Home
        public async Task<IActionResult> Index()
        {
            try
            {
                // Admin rolüne sahipse site seçimi zorunlu
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

                    return View();
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

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Home/Index hatası");
                return RedirectToAction("SignIn", "Auth");
            }
        }
    }
}
