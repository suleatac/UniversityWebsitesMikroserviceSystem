using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.SikcaSorulanSoru;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class SikcaSorulanSoruController : Controller
    {
        private readonly ISikcaSorulanSoruService _sikcaSorulanSoruService;
        private readonly ISikcaSorulanSoruKategoriService _kategoriService;
        private readonly ILogger<SikcaSorulanSoruController> _logger;

        public SikcaSorulanSoruController(
            ISikcaSorulanSoruService sikcaSorulanSoruService,
            ISikcaSorulanSoruKategoriService kategoriService,
            ILogger<SikcaSorulanSoruController> logger)
        {
            _sikcaSorulanSoruService = sikcaSorulanSoruService;
            _kategoriService = kategoriService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Sıkça Sorulan Soru listesi getiriliyor.");

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var result = await _sikcaSorulanSoruService.GetSikcaSorulanSorularAsync(currentSiteId, currentDilId);

            if (!result.IsSuccess)
            {
                _logger.LogError("Sıkça Sorulan Soru listesi alınamadı. Hata: {Error}", result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Sıkça Sorulan Soru listesi alınamadı.";
                return View(new List<GetSikcaSorulanSoruVm>());
            }

            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Sıkça Sorulan Soru oluşturma sayfası açıldı.");

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var kategoriler = await _kategoriService.GetSikcaSorulanSoruKategorilerAsync();

            var viewModel = new SikcaSorulanSoruCreateIndexVm
            {
                CreateSikcaSorulanSoru = new CreateSikcaSorulanSoruVm { SiteId = currentSiteId, DilId = currentDilId },
                Kategoriler = kategoriler.Data ?? new List<ViewModels.SikcaSorulanSoruKategori.GetSikcaSorulanSoruKategoriVm>(),
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SikcaSorulanSoruCreateIndexVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create SSS - ModelState geçersiz.");
                var kategoriler = await _kategoriService.GetSikcaSorulanSoruKategorilerAsync();
                model.Kategoriler = kategoriler.Data ?? new List<ViewModels.SikcaSorulanSoruKategori.GetSikcaSorulanSoruKategoriVm>();
                return View(model);
            }

            var result = await _sikcaSorulanSoruService.CreateSikcaSorulanSoruAsync(model.CreateSikcaSorulanSoru);

            if (!result.IsSuccess)
            {
                _logger.LogError("SSS oluşturulamadı. Hata: {Error}", result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Sıkça Sorulan Soru oluşturulamadı.");
                var kategoriler = await _kategoriService.GetSikcaSorulanSoruKategorilerAsync();
                model.Kategoriler = kategoriler.Data ?? new List<ViewModels.SikcaSorulanSoruKategori.GetSikcaSorulanSoruKategoriVm>();
                return View(model);
            }

            _logger.LogInformation("SSS oluşturuldu. Soru: {Soru}", model.CreateSikcaSorulanSoru.Soru);
            TempData["Success"] = "Sıkça Sorulan Soru başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("SSS düzenleme sayfası açıldı. Id: {Id}", id);

            var result = await _sikcaSorulanSoruService.GetSikcaSorulanSoruByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("SSS bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SikcaSorulanSoruDetailVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update SSS - ModelState geçersiz.");
                return View(model);
            }

            var result = await _sikcaSorulanSoruService.UpdateSikcaSorulanSoruAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("SSS güncellenemedi. Id: {Id}, Hata: {Error}", model.Id, result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");
                return View(model);
            }

            _logger.LogInformation("SSS güncellendi. Id: {Id}", model.Id);
            TempData["Success"] = "Sıkça Sorulan Soru başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("SSS delete sayfası açıldı. Id: {Id}", id);

            var result = await _sikcaSorulanSoruService.GetSikcaSorulanSoruByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("SSS bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Silinecek kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogWarning("SSS silme isteği alındı. Id: {Id}", id);

            var result = await _sikcaSorulanSoruService.DeleteSikcaSorulanSoruAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("SSS silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Silme işlemi başarısız";
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("SSS başarıyla silindi. Id: {Id}", id);
            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}