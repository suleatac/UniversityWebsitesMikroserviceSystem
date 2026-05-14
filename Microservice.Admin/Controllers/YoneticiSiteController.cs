using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.Site;
using Microservice.Admin.ViewModels.TumPersonel;
using Microservice.Admin.ViewModels.User;
using Microservice.Admin.ViewModels.YoneticiSite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class YoneticiSiteController : Controller
    {
        private readonly IYoneticiSiteService _yoneticiSiteService;

        private readonly ISiteService _siteService;
        private readonly ITumPersonelService _tumPersonelService;
        private readonly IUserService _userService;
        private readonly IKeycloakRoleService _keycloakRoleService;
        private readonly ILogger<YoneticiSiteController> _logger;

        public YoneticiSiteController(
            IYoneticiSiteService yoneticiSiteService,
         
            ITumPersonelService tumPersonelService,
            ISiteService siteService,
            IUserService userService,
            IKeycloakRoleService keycloakRoleService,
            ILogger<YoneticiSiteController> logger)
        {
            _yoneticiSiteService = yoneticiSiteService;
            _tumPersonelService = tumPersonelService;
            _siteService = siteService;
            _userService = userService;
            _keycloakRoleService = keycloakRoleService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            //// Admin henüz site eklememişse site create sayfasına yönlendir
            var sitesResult = await _siteService.GetSitesAsync();
            var hasSite = sitesResult.Data?.Any() == true;

            if (!hasSite)
            {
                _logger.LogWarning(
                    "Hiç site bulunamadı. Kullanıcı site oluşturma ekranına yönlendiriliyor.");

                return RedirectToAction("Create", "Site");
            }



            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetYoneticiSitesForPagination(
            int page = 1, int pageSize = 10, string search = "", int orderColumn = 0, string orderDir = "desc")
        {
            var result = await _yoneticiSiteService.GetYoneticiSitesAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError("YoneticiSite listesi alınamadı. Hata: {Error}", result.Fail?.Detail);
                return BadRequest(new { error = result.Fail?.Detail });
            }

            var data = result.Data ?? new List<YoneticiSiteDetailVm>();

            // Enrich with UserName from Keycloak
            var usersResult = await _userService.GetUsersAsync();
            var userLookup = usersResult.IsSuccess && usersResult.Data != null
                ? usersResult.Data.ToDictionary(u => u.Id, u => u.UserName)
                : new Dictionary<string, string>();

            foreach (var item in data)
            {
                if (!string.IsNullOrEmpty(item.KeycloakUserId) && userLookup.TryGetValue(item.KeycloakUserId, out var userName))
                {
                    item.UserName = userName;
                }
            }

            var columnName = orderColumn switch
            {
                1 => "SiteAdi",
                2 => "UserName",
                3 => "YoneticiTipiAdi",
                _ => "Id"
            };

            // Client-side pagination since the API returns all items
            var filteredData = string.IsNullOrEmpty(search)
                ? data
                : data.Where(x =>
                    (x.SiteAdi ?? "").Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    (x.UserName ?? "").Contains(search, StringComparison.OrdinalIgnoreCase) ).ToList();

            var orderedData = orderDir.ToLower() == "asc"
                ? columnName switch
                {
                    "SiteAdi" => filteredData.OrderBy(x => x.SiteAdi).ToList(),
                    "UserName" => filteredData.OrderBy(x => x.UserName).ToList(),
                    _ => filteredData.OrderBy(x => x.Id).ToList()
                }
                : columnName switch
                {
                    "SiteAdi" => filteredData.OrderByDescending(x => x.SiteAdi).ToList(),
                    "UserName" => filteredData.OrderByDescending(x => x.UserName).ToList(),
                    _ => filteredData.OrderByDescending(x => x.Id).ToList()
                };

            var pagedData = orderedData.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var totalCount = filteredData.Count;

            return Ok(new
            {
                data = pagedData,
                recordsTotal = totalCount,
                recordsFiltered = totalCount
            });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("YoneticiSite oluşturma sayfası açıldı.");

            // Admin henüz site seçmemişse site seçim sayfasına yönlendir
            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId");
     

            var tumPersoneller = await _tumPersonelService.GetTumPersonelsAsync();
            var tumSiteler = await _siteService.GetSitesAsync();

            var hasSite = tumSiteler.Data?.Any() == true;
            if (!hasSite)
            {
                _logger.LogWarning(
                    "Hiç site bulunamadı. Kullanıcı site oluşturma ekranına yönlendiriliyor.");

                return RedirectToAction("Create", "Site");
            }





            var usersResult = await _userService.GetUsersAsync();

            var viewModel = new YoneticiSiteIndexVm
            {
                TumPersoneller = tumPersoneller.Data ?? new List<GetPersonelVm>(),
                YoneticiSite = new YoneticiSiteVm { SiteId = currentSiteId ?? 0 },
                TumSiteler = tumSiteler.Data ?? new List<SiteGetVm>(),
                Users = usersResult.IsSuccess ? usersResult.Data ?? new List<UserListVm>() : new List<UserListVm>()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(YoneticiSiteIndexVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create YoneticiSite - ModelState geçersiz.");
             
                
                var tumPersoneller = await _tumPersonelService.GetTumPersonelsAsync();
                var tumSiteler = await _siteService.GetSitesAsync();
                var usersResult = await _userService.GetUsersAsync();

                model.TumPersoneller = tumPersoneller.Data ?? new List<GetPersonelVm>();
                model.TumSiteler = tumSiteler.Data ?? new List<SiteGetVm>();
                model.Users = usersResult.IsSuccess ? usersResult.Data ?? new List<UserListVm>() : new List<UserListVm>();

                return View(model);
            }

            // Admin rolüne sahip kullanıcılar için site ataması yapılmaz
            // Admin kullanıcılar zaten tüm sitelere erişebilir
            if (!string.IsNullOrEmpty(model.YoneticiSite.KeycloakUserId))
            {
                var isAdmin = await _keycloakRoleService.IsUserInRoleAsync(model.YoneticiSite.KeycloakUserId, "Admin");
                if (isAdmin)
                {
                    _logger.LogInformation("Admin rolüne sahip kullanıcı için site ataması atlanıyor. UserId: {UserId}", model.YoneticiSite.KeycloakUserId);
                    TempData["Warning"] = "Admin rolüne sahip kullanıcılar tüm sitelere zaten erişebilir. Site ataması gerekli değildir.";
                    return RedirectToAction(nameof(Index));
                }
            }

            var result = await _yoneticiSiteService.CreateYoneticiSiteAsync(model.YoneticiSite);

            if (!result.IsSuccess)
            {
                _logger.LogError("YoneticiSite oluşturulamadı. Hata: {Error}", result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "YoneticiSite oluşturulamadı.");
               
                var tumPersoneller = await _tumPersonelService.GetTumPersonelsAsync();
                var tumSiteler = await _siteService.GetSitesAsync();
                var usersResult = await _userService.GetUsersAsync();

                model.TumPersoneller = tumPersoneller.Data ?? new List<GetPersonelVm>();
                model.TumSiteler = tumSiteler.Data ?? new List<SiteGetVm>();
                model.Users = usersResult.IsSuccess ? usersResult.Data ?? new List<UserListVm>() : new List<UserListVm>();

                return View(model);
            }

            _logger.LogInformation("YoneticiSite oluşturuldu.");
            TempData["Success"] = "Yönetici Site başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("YoneticiSite düzenleme sayfası açıldı. Id: {Id}", id);

            var result = await _yoneticiSiteService.GetYoneticiSiteByIdAsync(id);

            var tumPersoneller = await _tumPersonelService.GetTumPersonelsAsync();
            var tumSiteler = await _siteService.GetSitesAsync();
            var usersResult = await _userService.GetUsersAsync();

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("YoneticiSite bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new YoneticiSiteEditIndexVm
            {
                TumPersoneller = tumPersoneller.Data ?? new List<GetPersonelVm>(),
                TumSiteler = tumSiteler.Data ?? new List<SiteGetVm>(),
                EditYoneticiSite = result.Data,
                Users = usersResult.IsSuccess ? usersResult.Data ?? new List<UserListVm>() : new List<UserListVm>()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(YoneticiSiteEditIndexVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update YoneticiSite - ModelState geçersiz.");

                var tumPersoneller = await _tumPersonelService.GetTumPersonelsAsync();
                var tumSiteler = await _siteService.GetSitesAsync();
                var usersResult = await _userService.GetUsersAsync();

                model.TumPersoneller = tumPersoneller.Data ?? new List<GetPersonelVm>();
                model.TumSiteler = tumSiteler.Data ?? new List<SiteGetVm>();
                model.Users = usersResult.IsSuccess ? usersResult.Data ?? new List<UserListVm>() : new List<UserListVm>();

                return View(model);
            }

            // Admin rolüne sahip kullanıcılar için site ataması güncellenmez
            if (!string.IsNullOrEmpty(model.EditYoneticiSite.KeycloakUserId))
            {
                var isAdminEdit = await _keycloakRoleService.IsUserInRoleAsync(model.EditYoneticiSite.KeycloakUserId, "Admin");
                if (isAdminEdit)
                {
                    _logger.LogInformation("Admin rolüne sahip kullanıcı için site ataması güncellenemedi. UserId: {UserId}", model.EditYoneticiSite.KeycloakUserId);
                    TempData["Warning"] = "Admin rolüne sahip kullanıcılar tüm sitelere zaten erişebilir. Site ataması güncellenemez.";
                    return RedirectToAction(nameof(Index));
                }
            }

            var result = await _yoneticiSiteService.UpdateYoneticiSiteAsync(model.EditYoneticiSite);

            if (!result.IsSuccess)
            {
                _logger.LogError("YoneticiSite güncellenemedi. Id: {Id}, Hata: {Error}", model.EditYoneticiSite.Id, result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");

                var tumPersoneller = await _tumPersonelService.GetTumPersonelsAsync();
                var tumSiteler = await _siteService.GetSitesAsync();
                var usersResult = await _userService.GetUsersAsync();

                model.TumPersoneller = tumPersoneller.Data ?? new List<GetPersonelVm>();
                model.TumSiteler = tumSiteler.Data ?? new List<SiteGetVm>();
                model.Users = usersResult.IsSuccess ? usersResult.Data ?? new List<UserListVm>() : new List<UserListVm>();

                return View(model);
            }

            _logger.LogInformation("YoneticiSite güncellendi. Id: {Id}", model.EditYoneticiSite.Id);
            TempData["Success"] = "Yönetici Site başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("YoneticiSite delete sayfası açıldı. Id: {Id}", id);

            var result = await _yoneticiSiteService.GetYoneticiSiteByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("YoneticiSite bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Silinecek kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogWarning("YoneticiSite silme isteği alındı. Id: {Id}", id);

            var result = await _yoneticiSiteService.DeleteYoneticiSiteAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("YoneticiSite silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Silme işlemi başarısız";
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("YoneticiSite başarıyla silindi. Id: {Id}", id);
            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}