using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.YoneticiSite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class YoneticiSiteController : Controller
    {
        private readonly IYoneticiSiteService _yoneticiSiteService;
        private readonly IYoneticiTipiService _yoneticiTipiService;
        private readonly ILogger<YoneticiSiteController> _logger;

        public YoneticiSiteController(
            IYoneticiSiteService yoneticiSiteService,
            IYoneticiTipiService yoneticiTipiService,
            ILogger<YoneticiSiteController> logger)
        {
            _yoneticiSiteService = yoneticiSiteService;
            _yoneticiTipiService = yoneticiTipiService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetYoneticiSitesForPagination(
            int page = 1, int pageSize = 10, string search = "", int orderColumn = 0, string orderDir = "desc")
        {
            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;

            var columnName = orderColumn switch
            {
                1 => "KeycloakUserId",
                2 => "YoneticiTipiAdi",
                _ => "Id"
            };

            var result = await _yoneticiSiteService.GetYoneticiSitesAsync(currentSiteId);

            if (!result.IsSuccess)
            {
                _logger.LogError("YoneticiSite listesi alınamadı. Hata: {Error}", result.Fail?.Detail);
                return BadRequest(new { error = result.Fail?.Detail });
            }

            var data = result.Data ?? new List<GetYoneticiSiteVm>();

            // Client-side pagination since the API returns all items
            var filteredData = string.IsNullOrEmpty(search)
                ? data
                : data.Where(x => x.KeycloakUserId.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                  x.YoneticiTipiAdi.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            var orderedData = orderDir.ToLower() == "asc"
                ? columnName switch
                {
                    "KeycloakUserId" => filteredData.OrderBy(x => x.KeycloakUserId).ToList(),
                    "YoneticiTipiAdi" => filteredData.OrderBy(x => x.YoneticiTipiAdi).ToList(),
                    _ => filteredData.OrderBy(x => x.Id).ToList()
                }
                : columnName switch
                {
                    "KeycloakUserId" => filteredData.OrderByDescending(x => x.KeycloakUserId).ToList(),
                    "YoneticiTipiAdi" => filteredData.OrderByDescending(x => x.YoneticiTipiAdi).ToList(),
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

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var yoneticiTipleri = await _yoneticiTipiService.GetYoneticiTipleriAsync();

            var viewModel = new YoneticiSiteCreateIndexVm
            {
                CreateYoneticiSite = new CreateYoneticiSiteVm { SiteId = currentSiteId },
                YoneticiTipleri = yoneticiTipleri.Data ?? new List<ViewModels.YoneticiTipi.GetYoneticiTipiVm>(),
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(YoneticiSiteCreateIndexVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create YoneticiSite - ModelState geçersiz.");
                var yoneticiTipleri = await _yoneticiTipiService.GetYoneticiTipleriAsync();
                model.YoneticiTipleri = yoneticiTipleri.Data ?? new List<ViewModels.YoneticiTipi.GetYoneticiTipiVm>();
                return View(model);
            }

            var result = await _yoneticiSiteService.CreateYoneticiSiteAsync(model.CreateYoneticiSite);

            if (!result.IsSuccess)
            {
                _logger.LogError("YoneticiSite oluşturulamadı. Hata: {Error}", result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "YoneticiSite oluşturulamadı.");
                var yoneticiTipleri = await _yoneticiTipiService.GetYoneticiTipleriAsync();
                model.YoneticiTipleri = yoneticiTipleri.Data ?? new List<ViewModels.YoneticiTipi.GetYoneticiTipiVm>();
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
            var yoneticiTipleri = await _yoneticiTipiService.GetYoneticiTipleriAsync();

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("YoneticiSite bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new YoneticiSiteEditIndexVm
            {
                YoneticiSite = result.Data,
                YoneticiTipleri = yoneticiTipleri.Data ?? new List<ViewModels.YoneticiTipi.GetYoneticiTipiVm>(),
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
                var yoneticiTipleri = await _yoneticiTipiService.GetYoneticiTipleriAsync();
                model.YoneticiTipleri = yoneticiTipleri.Data ?? new List<ViewModels.YoneticiTipi.GetYoneticiTipiVm>();
                return View(model);
            }

            var result = await _yoneticiSiteService.UpdateYoneticiSiteAsync(model.YoneticiSite);

            if (!result.IsSuccess)
            {
                _logger.LogError("YoneticiSite güncellenemedi. Id: {Id}, Hata: {Error}", model.YoneticiSite.Id, result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");
                var yoneticiTipleri = await _yoneticiTipiService.GetYoneticiTipleriAsync();
                model.YoneticiTipleri = yoneticiTipleri.Data ?? new List<ViewModels.YoneticiTipi.GetYoneticiTipiVm>();
                return View(model);
            }

            _logger.LogInformation("YoneticiSite güncellendi. Id: {Id}", model.YoneticiSite.Id);
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