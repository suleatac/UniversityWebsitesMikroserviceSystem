using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.SikcaSorulanSoruKategori;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class SikcaSorulanSoruKategoriController : Controller
    {
        private readonly ISikcaSorulanSoruKategoriService _kategoriService;
        private readonly ILogger<SikcaSorulanSoruKategoriController> _logger;

        public SikcaSorulanSoruKategoriController(
            ISikcaSorulanSoruKategoriService kategoriService,
            ILogger<SikcaSorulanSoruKategoriController> logger)
        {
            _kategoriService = kategoriService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("SSS Kategori listesi getiriliyor.");

            var result = await _kategoriService.GetSikcaSorulanSoruKategorilerAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError("SSS Kategori listesi alınamadı. Hata: {Error}", result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Kategori listesi alınamadı.";
                return View(new List<GetSikcaSorulanSoruKategoriVm>());
            }

            return View(result.Data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            _logger.LogInformation("SSS Kategori oluşturma sayfası açıldı.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SikcaSorulanSoruKategoriVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create SSS Kategori - ModelState geçersiz.");
                return View(model);
            }

            var result = await _kategoriService.CreateSikcaSorulanSoruKategoriAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("SSS Kategori oluşturulamadı. Hata: {Error}", result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Kategori oluşturulamadı.");
                return View(model);
            }

            _logger.LogInformation("SSS Kategori oluşturuldu. Ad: {Ad}", model.Ad);
            TempData["Success"] = "Kategori başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("SSS Kategori düzenleme sayfası açıldı. Id: {Id}", id);

            var result = await _kategoriService.GetSikcaSorulanSoruKategoriByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("SSS Kategori bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SikcaSorulanSoruKategoriVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update SSS Kategori - ModelState geçersiz. Id: {Id}", model.Id);
                return View(model);
            }

            var result = await _kategoriService.UpdateSikcaSorulanSoruKategoriAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("SSS Kategori güncellenemedi. Id: {Id}, Hata: {Error}", model.Id, result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");
                return View(model);
            }

            _logger.LogInformation("SSS Kategori güncellendi. Id: {Id}", model.Id);
            TempData["Success"] = "Kategori başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("SSS Kategori delete sayfası açıldı. Id: {Id}", id);

            var result = await _kategoriService.GetSikcaSorulanSoruKategoriByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("SSS Kategori bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Silinecek kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogWarning("SSS Kategori silme isteği alındı. Id: {Id}", id);

            var result = await _kategoriService.DeleteSikcaSorulanSoruKategoriAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("SSS Kategori silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Silme işlemi başarısız";
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("SSS Kategori başarıyla silindi. Id: {Id}", id);
            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}