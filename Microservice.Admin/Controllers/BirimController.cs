using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.Birim;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class BirimController : Controller
    {
        private readonly IBirimService _birimService;
        private readonly ILogger<BirimController> _logger;

        public BirimController(IBirimService birimService, ILogger<BirimController> logger)
        {
            _birimService = birimService;
            _logger = logger;
        }

        // 🔹 LIST
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Birim listesi getiriliyor.");

            var result = await _birimService.GetBirimsAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError("Birim listesi alınamadı. Hata: {Error}", result.Fail?.Detail);

                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Birim listesi alınamadı.";
                return View("Error");
            }

            _logger.LogInformation("Birim listesi başarıyla getirildi. Count: {Count}", result.Data!.Count);

            return View(result.Data);
        }

        // 🔹 CREATE - GET
        [HttpGet]
        public IActionResult Create()
        {
            _logger.LogInformation("Birim oluşturma sayfası açıldı.");
            return View();
        }

        // 🔹 CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBirimVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create Birim - ModelState geçersiz.");
                return View(model);
            }

            var result = await _birimService.CreateBirimAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("Birim oluşturulamadı. Hata: {Error}", result.Fail?.Detail);

                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Birim oluşturulamadı.");
                return View(model);
            }

            _logger.LogInformation("Birim oluşturuldu.");

            TempData["Success"] = "Birim başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 UPDATE - GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Birim edit sayfası açıldı. Id: {Id}", id);

            var result = await _birimService.GetBirimByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Birim bulunamadı. Id: {Id}", id);

                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            // Detail -> Update VM mapping gerekiyorsa burada yap
            var updateVm = new UpdateBirimVm {
                Id = result.Data.Id,
                Ad = result.Data.Ad
                // diğer alanları ekle
            };

            return View(updateVm);
        }

        // 🔹 UPDATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateBirimVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update Birim - ModelState geçersiz.");
                return View(model);
            }

            var result = await _birimService.UpdateBirimAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("Birim güncellenemedi. Id: {Id}, Hata: {Error}", model.Id, result.Fail?.Detail);

                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");
                return View(model);
            }

            _logger.LogInformation("Birim güncellendi. Id: {Id}", model.Id);

            TempData["Success"] = "Birim başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 DELETE - GET
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Birim delete sayfası açıldı. Id: {Id}", id);

            var result = await _birimService.GetBirimByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Birim bulunamadı. Id: {Id}", id);

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
            _logger.LogWarning("Birim silme isteği alındı. Id: {Id}", id);

            var result = await _birimService.DeleteBirimAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Birim silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);

                TempData["Error"] = result.Fail?.Detail
                                    ?? result.Fail?.Title
                                    ?? "Silme işlemi başarısız";

                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Birim başarıyla silindi. Id: {Id}", id);

            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}