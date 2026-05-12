using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.SiteSelection;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.ViewComponents
{
    public class SiteSelectionViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;
        private readonly IDilService _dilService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IYoneticiSiteService _yoneticiSiteService;
        private readonly ILogger<SiteSelectionViewComponent> _logger;

        public SiteSelectionViewComponent(
            ISiteService siteService,
            IDilService dilService,
            ICurrentUserService currentUserService,
            IYoneticiSiteService yoneticiSiteService,
            ILogger<SiteSelectionViewComponent> logger)
        {
            _siteService = siteService;
            _dilService = dilService;
            _currentUserService = currentUserService;
            _yoneticiSiteService = yoneticiSiteService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Session'dan mevcut site ve dil bilgilerini al, yoksa default 1
            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var model = new SiteSelectionVm
            {
                CurrentSiteId = currentSiteId,
                CurrentDilId = currentDilId
            };

            // Admin rolüne göre site filtreleme
            if (_currentUserService.IsAuthenticated && _currentUserService.IsAdmin)
            {
                // Admin tüm siteleri görebilir
                model.IsAdmin = true;
                var sitesResult = await _siteService.GetSitesAsync();
                if (sitesResult.IsSuccess && sitesResult.Data != null)
                {
                    model.Sites = sitesResult.Data;
                }
                else
                {
                    _logger.LogWarning("Site listesi yüklenemedi. Varsayılan boş liste kullanılıyor.");
                    model.Sites = new List<ViewModels.Site.SiteGetVm>();
                }
            }
            else if (_currentUserService.IsAuthenticated)
            {
                // Admin olmayan kullanıcı sadece yetkili olduğu siteleri görebilir
                model.IsAdmin = false;
                try
                {
                    var keycloakUserId = _currentUserService.KeycloakUserId;
                    var authorizedSitesResult = await _yoneticiSiteService.GetYoneticiSitesByKeycloakUserIdAsync(keycloakUserId);

                    if (authorizedSitesResult.IsSuccess && authorizedSitesResult.Data != null)
                    {
                        var authorizedSiteIds = authorizedSitesResult.Data.Select(s => s.SiteId).ToList();
                        model.AuthorizedSiteIds = authorizedSiteIds;

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
                    }
                    else
                    {
                        _logger.LogWarning("Kullanıcının yetkili siteleri bulunamadı. KeycloakUserId: {KeycloakUserId}", keycloakUserId);
                        model.Sites = new List<ViewModels.Site.SiteGetVm>();
                        model.AuthorizedSiteIds = new List<int>();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Yetkili siteler yüklenirken hata oluştu.");
                    model.Sites = new List<ViewModels.Site.SiteGetVm>();
                    model.AuthorizedSiteIds = new List<int>();
                }
            }
            else
            {
                model.IsAdmin = false;
                model.Sites = new List<ViewModels.Site.SiteGetVm>();
            }

            // Dilleri API'den çek
            var dillerResult = await _dilService.GetDilsAsync();
            if (dillerResult.IsSuccess && dillerResult.Data != null)
            {
                model.Diller = dillerResult.Data;
            }
            else
            {
                _logger.LogWarning("Dil listesi yüklenemedi. Varsayılan boş liste kullanılıyor.");
                model.Diller = new List<ViewModels.Dil.GetDilVm>();
            }

            // Mevcut site adını bul
            var currentSite = model.Sites.FirstOrDefault(s => s.Id == currentSiteId);
            model.CurrentSiteName = currentSite?.SiteAdi ?? $"Site {currentSiteId}";

            // Mevcut dil adını bul
            var currentDil = model.Diller.FirstOrDefault(d => d.Id == currentDilId);
            model.CurrentDilName = currentDil?.Ad ?? $"Dil {currentDilId}";

            return View(model);
        }
    }
}