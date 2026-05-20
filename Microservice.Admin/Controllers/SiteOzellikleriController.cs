using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.SiteOzellikleri;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class SiteOzellikleriController : Controller
    {
        private readonly ISiteOzellikleriService _siteOzellikleriService;
        private readonly ILogger<SiteOzellikleriController> _logger;

        public SiteOzellikleriController(
            ISiteOzellikleriService siteOzellikleriService,
            ILogger<SiteOzellikleriController> logger)
        {
            _siteOzellikleriService = siteOzellikleriService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("SiteOzellikleri sayfası açıldı.");
            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;

            var result = await _siteOzellikleriService.GetSiteOzellikleriAsync(currentSiteId);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("SiteOzellikleri bulunamadı. SiteId: {SiteId}", currentSiteId);

                return View(new SiteOzellikleriVm { SiteId = currentSiteId });
            }

            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            _logger.LogInformation("SiteOzellikleri düzenleme sayfası açıldı.");
            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;

            var result = await _siteOzellikleriService.GetSiteOzellikleriAsync(currentSiteId);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("SiteOzellikleri bulunamadı. SiteId: {SiteId}", currentSiteId);
                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SiteOzellikleriVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update SiteOzellikleri - ModelState geçersiz.");
                return View(model);
            }

            var result = await _siteOzellikleriService.UpdateSiteOzellikleriAsync(model.Id, model);

            if (!result.IsSuccess)
            {
                _logger.LogError("SiteOzellikleri güncellenemedi. Id: {Id}, Hata: {Error}", model.Id, result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");
                return View(model);
            }

            _logger.LogInformation("SiteOzellikleri güncellendi. Id: {Id}", model.Id);
            TempData["Success"] = "Site özellikleri başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("SiteOzellikleri oluşturma sayfası açıldı.");
            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;

            var model = new SiteOzellikleriVm { SiteId = currentSiteId };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SiteOzellikleriVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create SiteOzellikleri - ModelState geçersiz.");
                return View(model);
            }

            var result = await _siteOzellikleriService.CreateSiteOzellikleriAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("SiteOzellikleri oluşturulamadı. Hata: {Error}", result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Site özellikleri oluşturulamadı.");
                return View(model);
            }

            _logger.LogInformation("SiteOzellikleri oluşturuldu.");
            TempData["Success"] = "Site özellikleri başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("SiteOzellikleri delete sayfası açıldı. Id: {Id}", id);

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var result = await _siteOzellikleriService.GetSiteOzellikleriAsync(currentSiteId);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("SiteOzellikleri bulunamadı.");
                TempData["Error"] = "Silinecek kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogWarning("SiteOzellikleri silme isteği alındı. Id: {Id}", id);

            var result = await _siteOzellikleriService.DeleteSiteOzellikleriAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("SiteOzellikleri silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Silme işlemi başarısız";
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("SiteOzellikleri başarıyla silindi. Id: {Id}", id);
            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}