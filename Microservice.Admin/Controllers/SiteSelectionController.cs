using Microservice.Admin.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class SiteSelectionController : Controller
    {
        private readonly ISiteService _siteService;
        private readonly IDilService _dilService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IYoneticiSiteService _yoneticiSiteService;
        private readonly ILogger<SiteSelectionController> _logger;

        public SiteSelectionController(
            ISiteService siteService,
            IDilService dilService,
            ICurrentUserService currentUserService,
            IYoneticiSiteService yoneticiSiteService,
            ILogger<SiteSelectionController> logger)
        {
            _siteService = siteService;
            _dilService = dilService;
            _currentUserService = currentUserService;
            _yoneticiSiteService = yoneticiSiteService;
            _logger = logger;
        }

        /// <summary>
        /// Kullanıcıların site seçimi yapması için zorunlu sayfa.
        /// Admin kullanıcılar tüm siteleri görebilir.
        /// Admin olmayan kullanıcılar sadece yetkili oldukları siteleri görebilir.
        /// Eğer kullanıcı henüz site seçmemişse bu sayfaya yönlendirilir.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> SelectSite()
        {
            // Zaten bir site seçilmişse ana sayfaya yönlendir
            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId");
            if (currentSiteId.HasValue && currentSiteId.Value > 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new ViewModels.SiteSelection.SiteSelectionVm
            {
                MustSelectSite = true,
                CurrentSiteId = 0,
                CurrentDilId = 1,
                CurrentSiteName = "Site Seç",
                CurrentDilName = "Türkçe"
            };

            if (_currentUserService.IsAdmin)
            {
                // Admin tüm siteleri görebilir
                model.IsAdmin = true;

                var sitesResult = await _siteService.GetSitesAsync();
                if (sitesResult.IsSuccess && sitesResult.Data != null)
                {
                    model.Sites = sitesResult.Data;
                    if (!model.Sites.Any())
                    {
                        model.NoSitesAvailable = true;
                    }
                }
                else
                {
                    model.Sites = new List<ViewModels.Site.SiteGetVm>();
                    model.NoSitesAvailable = true;
                }
            }
            else
            {
                // Admin olmayan kullanıcı sadece yetkili olduğu siteleri görebilir
                model.IsAdmin = false;

                try
                {
                    var keycloakUserId = _currentUserService.KeycloakUserId;
                    var authorizedSitesResult = await _yoneticiSiteService.GetYoneticiSitesByKeycloakUserIdAsync(keycloakUserId);

                    if (authorizedSitesResult.IsSuccess && authorizedSitesResult.Data != null && authorizedSitesResult.Data.Any())
                    {
                        var authorizedSiteIds = authorizedSitesResult.Data.Select(s => s.SiteId).ToList();
                        model.AuthorizedSiteIds = authorizedSiteIds;

                        // Yetkili site ID'lerini session'a kaydet (site seçimi doğrulaması için)
                        HttpContext.Session.SetString("AuthorizedSiteIds", string.Join(",", authorizedSiteIds));

                        // Tüm siteleri çek, ama sadece yetkili olanları göster
                        var allSitesResult = await _siteService.GetSitesAsync();
                        if (allSitesResult.IsSuccess && allSitesResult.Data != null)
                        {
                            model.Sites = allSitesResult.Data.Where(s => authorizedSiteIds.Contains(s.Id)).ToList();
                        }
                        else
                        {
                            model.Sites = new List<ViewModels.Site.SiteGetVm>();
                        }

                        if (!model.Sites.Any())
                        {
                            model.NoSitesAvailable = true;
                        }
                    }
                    else
                    {
                        // Yetkili site yoksa
                        _logger.LogWarning("Kullanıcının yetkili siteleri bulunamadı. KeycloakUserId: {KeycloakUserId}", keycloakUserId);
                        model.Sites = new List<ViewModels.Site.SiteGetVm>();
                        model.AuthorizedSiteIds = new List<int>();
                        model.NoSitesAvailable = true;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Yetkili siteler yüklenirken hata oluştu.");
                    model.Sites = new List<ViewModels.Site.SiteGetVm>();
                    model.AuthorizedSiteIds = new List<int>();
                    model.NoSitesAvailable = true;
                }
            }

            // Dilleri yükle
            var dillerResult = await _dilService.GetDilsAsync();
            if (dillerResult.IsSuccess && dillerResult.Data != null)
            {
                model.Diller = dillerResult.Data;
            }
            else
            {
                model.Diller = new List<ViewModels.Dil.GetDilVm>();
            }

            return View(model);
        }

        /// <summary>
        /// Site seçimini kaydeder ve ana sayfaya yönlendirir.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectSite(int siteId, int dilId = 1)
        {
            if (siteId <= 0)
            {
                TempData["Error"] = "Lütfen bir site seçiniz.";
                return RedirectToAction(nameof(SelectSite));
            }

            // Admin olmayan kullanıcı sadece yetkisi olan siteyi seçebilir
            if (!_currentUserService.IsAdmin)
            {
                var authorizedSiteIdsStr = HttpContext.Session.GetString("AuthorizedSiteIds");
                if (!string.IsNullOrEmpty(authorizedSiteIdsStr))
                {
                    var authorizedIds = authorizedSiteIdsStr.Split(',').Select(int.Parse).ToList();
                    if (!authorizedIds.Contains(siteId))
                    {
                        _logger.LogWarning("Admin olmayan kullanıcı yetkisi olmayan siteyi seçmeye çalıştı. SiteId: {SiteId}", siteId);
                        TempData["Error"] = "Bu siteyi seçme yetkiniz bulunmamaktadır.";
                        return RedirectToAction(nameof(SelectSite));
                    }
                }
            }

            HttpContext.Session.SetInt32("CurrentSiteId", siteId);
            HttpContext.Session.SetInt32("CurrentDilId", dilId);

            _logger.LogInformation("Kullanıcı site seçimi yaptı. SiteId: {SiteId}, DilId: {DilId}, User: {User}", siteId, dilId, _currentUserService.Username);

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Session'a seçilen site Id'sini kaydeder. (AJAX çağrısı)
        /// </summary>
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult SetSite(int siteId)
        {
            if (siteId <= 0)
            {
                return Json(new { success = false, message = "Geçersiz site Id." });
            }

            HttpContext.Session.SetInt32("CurrentSiteId", siteId);
            return Json(new { success = true, message = "Site seçimi güncellendi.", siteId = siteId });
        }

        /// <summary>
        /// Session'a seçilen dil Id'sini kaydeder. (AJAX çağrısı)
        /// </summary>
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult SetDil(int dilId)
        {
            if (dilId <= 0)
            {
                return Json(new { success = false, message = "Geçersiz dil Id." });
            }

            HttpContext.Session.SetInt32("CurrentDilId", dilId);
            return Json(new { success = true, message = "Dil seçimi güncellendi.", dilId = dilId });
        }

        /// <summary>
        /// Mevcut site ve dil seçimini JSON olarak döner.
        /// </summary>
        [HttpGet]
        public IActionResult GetCurrentSelection()
        {
            var siteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 0;
            var dilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            return Json(new { siteId, dilId });
        }
    }
}