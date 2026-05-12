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
                // Admin rolüne sahipse tüm siteleri görebilir, default siteId=1 ve dilId=1
                if (_currentUserService.IsAdmin)
                {
                    if (!HttpContext.Session.GetInt32("CurrentSiteId").HasValue)
                    {
                        HttpContext.Session.SetInt32("CurrentSiteId", 1);
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

                // Yetkili siteler varsa, session'a ilk yetkili siteyi set et
                if (!HttpContext.Session.GetInt32("CurrentSiteId").HasValue)
                {
                    var firstAuthorizedSite = authorizedSitesResult.Data.First();
                    HttpContext.Session.SetInt32("CurrentSiteId", firstAuthorizedSite.SiteId);
                }
                if (!HttpContext.Session.GetInt32("CurrentDilId").HasValue)
                {
                    HttpContext.Session.SetInt32("CurrentDilId", 1);
                }

                // Yetkili site ID'lerini session'a kaydet (site seçimi filtrelemesi için)
                var authorizedSiteIds = authorizedSitesResult.Data.Select(s => s.SiteId).ToList();
                HttpContext.Session.SetString("AuthorizedSiteIds", string.Join(",", authorizedSiteIds));

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
