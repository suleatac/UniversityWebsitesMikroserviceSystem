using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Banner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class BannerController : Controller
    {
        private readonly IBannerService _bannerService;
        private readonly ISiteService _siteService;
        private readonly IDilService _dilService;
        private readonly IHedefService _hedefService;
        private readonly ILogger<BannerController> _logger;

        public BannerController(
            IBannerService bannerService,
            ISiteService siteService,
            IDilService dilService,
            IHedefService hedefService,
            ILogger<BannerController> logger)
        {
            _bannerService = bannerService;
            _siteService = siteService;
            _dilService = dilService;
            _hedefService = hedefService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var result = await _bannerService.GetBannersAsync(currentSiteId, currentDilId);

            if (!result.IsSuccess)
            {
                _logger.LogError("Banner listesi alınamadı. Hata: {Error}", result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Banner listesi alınamadı.";
                return View(new List<GetBannerVm>());
            }

            return View(result.Data ?? new List<GetBannerVm>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReorderBanners([FromBody] List<ReorderBannerItemVm> items)
        {
            if (items == null || items.Count == 0)
            {
                return Json(new { success = false, message = "Geçersiz veri." });
            }

            _logger.LogInformation("Banner sıralaması güncelleniyor. {Count} öğe.", items.Count);

            var result = await _bannerService.ReorderBannersAsync(items);

            if (!result.IsSuccess)
            {
                _logger.LogError("Banner sıralaması güncellenemedi. Hata: {Error}", result.Fail?.Detail);
                return Json(new { success = false, message = result.Fail?.Detail ?? "Sıralama güncellenemedi." });
            }

            _logger.LogInformation("Banner sıralaması başarıyla güncellendi.");
            return Json(new { success = true, message = "Sıralama başarıyla güncellendi." });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Banner oluşturma sayfası açıldı.");

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var hedefler = await _hedefService.GetHedefsAsync();

            var viewModel = new BannerCreateIndexVm
            {
                CreateBanner = new CreateBannerVm { SiteId = currentSiteId, DilId = currentDilId },
                Hedefler = hedefler.Data ?? new List<ViewModels.Hedef.GetHedefVm>(),
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BannerCreateIndexVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create Banner - ModelState geçersiz.");
                var hedefler = await _hedefService.GetHedefsAsync();
                model.Hedefler = hedefler.Data ?? new List<ViewModels.Hedef.GetHedefVm>();
                return View(model);
            }

            var result = await _bannerService.CreateBannerAsync(model.CreateBanner);

            if (!result.IsSuccess)
            {
                _logger.LogError("Banner oluşturulamadı. Hata: {Error}", result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Banner oluşturulamadı.");
                var hedefler = await _hedefService.GetHedefsAsync();
                model.Hedefler = hedefler.Data ?? new List<ViewModels.Hedef.GetHedefVm>();
                return View(model);
            }

            _logger.LogInformation("Banner oluşturuldu. Başlık: {Title}", model.CreateBanner.Baslik);
            TempData["Success"] = "Banner başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Banner düzenleme sayfası açıldı. Id: {Id}", id);

            var result = await _bannerService.GetBannerByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Banner bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BannerDetailVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update Banner - ModelState geçersiz.");
                return View(model);
            }

            var result = await _bannerService.UpdateBannerAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("Banner güncellenemedi. Id: {Id}, Hata: {Error}", model.Id, result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");
                return View(model);
            }

            _logger.LogInformation("Banner güncellendi. Id: {Id}", model.Id);
            TempData["Success"] = "Banner başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Banner delete sayfası açıldı. Id: {Id}", id);

            var result = await _bannerService.GetBannerByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Banner bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Silinecek kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogWarning("Banner silme isteği alındı. Id: {Id}", id);

            var result = await _bannerService.DeleteBannerAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Banner silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Silme işlemi başarısız";
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Banner başarıyla silindi. Id: {Id}", id);
            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
