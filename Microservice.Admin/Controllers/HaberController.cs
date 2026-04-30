using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.Haber;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class HaberController : Controller
    {
        private readonly IHaberService _haberService;
        private readonly ILogger<HaberController> _logger;

        public HaberController(IHaberService haberService, ILogger<HaberController> logger)
        {
            _haberService = haberService;
            _logger = logger;
        }

        // 🔹 LIST
        public async Task<IActionResult> Index(int siteId=1, int dilId=1)
        {
            _logger.LogInformation("Haber listesi getiriliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);

            var result = await _haberService.GetHabersAsync(siteId, dilId);

            if (!result.IsSuccess)
            {
                _logger.LogError("Haber listesi alınamadı. Hata: {Error}", result.Fail?.Detail);

                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Haber listesi alınamadı.";
                return View("Error");
            }

            _logger.LogInformation("Haber listesi başarıyla getirildi. Count: {Count}", result.Data!.Count);

            return View(result.Data);
        }

        // 🔹 DETAIL - GET
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Haber detayı getiriliyor. Id: {Id}", id);

            var result = await _haberService.GetHaberByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Haber bulunamadı. Id: {Id}", id);

                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        // 🔹 CREATE - GET
        [HttpGet]
        public IActionResult Create()
        {
            _logger.LogInformation("Haber oluşturma sayfası açıldı.");
            return View();
        }

        // 🔹 CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateHaberVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create Haber - ModelState geçersiz.");
                return View(model);
            }

            var result = await _haberService.CreateHaberAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("Haber oluşturulamadı. Hata: {Error}", result.Fail?.Detail);

                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Haber oluşturulamadı.");
                return View(model);
            }

            _logger.LogInformation("Haber oluşturuldu. Başlık: {Title}", model.Baslik);

            TempData["Success"] = "Haber başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index), new { siteId = model.SiteId, dilId = model.DilId });
        }

        // 🔹 UPDATE - GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Haber edit sayfası açıldı. Id: {Id}", id);

            var result = await _haberService.GetHaberByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Haber bulunamadı. Id: {Id}", id);

                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        // 🔹 UPDATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HaberDetailVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update Haber - ModelState geçersiz.");
                return View(model);
            }

            var result = await _haberService.UpdateHaberAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("Haber güncellenemedi. Id: {Id}, Hata: {Error}", model.Id, result.Fail?.Detail);

                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");
                return View(model);
            }

            _logger.LogInformation("Haber güncellendi. Id: {Id}", model.Id);

            TempData["Success"] = "Haber başarıyla güncellendi.";
            return RedirectToAction(nameof(Index), new { siteId = model.SiteId, dilId = model.DilId });
        }

        // 🔹 DELETE - GET
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Haber delete sayfası açıldı. Id: {Id}", id);

            var result = await _haberService.GetHaberByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Haber bulunamadı. Id: {Id}", id);

                TempData["Error"] = "Silinecek kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        // 🔹 DELETE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogWarning("Haber silme isteği alındı. Id: {Id}", id);

            var result = await _haberService.DeleteHaberAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Haber silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);

                TempData["Error"] = result.Fail?.Detail
                                    ?? result.Fail?.Title
                                    ?? "Silme işlemi başarısız";

                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Haber başarıyla silindi. Id: {Id}", id);

            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
