using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.Popup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class PopupController : Controller
    {
        private readonly IPopupService _popupService;
        private readonly ILogger<PopupController> _logger;

        public PopupController(
            IPopupService popupService,
            ILogger<PopupController> logger)
        {
            _popupService = popupService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Popup sayfası açıldı.");
            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;

            var result = await _popupService.GetPopupBySiteIdAsync(currentSiteId);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Popup bulunamadı. SiteId: {SiteId}", currentSiteId);
                TempData["Error"] = result.Fail?.Detail ?? "Popup bulunamadı.";
                return View(new PopupDetailVm { SiteId = currentSiteId });
            }

            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            _logger.LogInformation("Popup düzenleme sayfası açıldı.");
            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;

            var result = await _popupService.GetPopupBySiteIdAsync(currentSiteId);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Popup bulunamadı. SiteId: {SiteId}", currentSiteId);
                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PopupDetailVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update Popup - ModelState geçersiz.");
                return View(model);
            }

            var result = await _popupService.UpdatePopupAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("Popup güncellenemedi. Id: {Id}, Hata: {Error}", model.Id, result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");
                return View(model);
            }

            _logger.LogInformation("Popup güncellendi. Id: {Id}", model.Id);
            TempData["Success"] = "Popup başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Popup oluşturma sayfası açıldı.");
            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;

            // Check if a popup already exists for this site (one-to-one)
            var existingPopup = await _popupService.GetPopupBySiteIdAsync(currentSiteId);
            if (existingPopup.IsSuccess && existingPopup.Data != null)
            {
                TempData["Warning"] = "Bu site için zaten bir popup mevcut. Mevcut popup'ı düzenleyebilirsiniz.";
                return RedirectToAction(nameof(Edit));
            }

            var model = new CreatePopupVm { SiteId = currentSiteId };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePopupVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create Popup - ModelState geçersiz.");
                return View(model);
            }

            var result = await _popupService.CreatePopupAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("Popup oluşturulamadı. Hata: {Error}", result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Popup oluşturulamadı.");
                return View(model);
            }

            _logger.LogInformation("Popup oluşturuldu. Başlık: {Title}", model.Baslik);
            TempData["Success"] = "Popup başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Popup delete sayfası açıldı. Id: {Id}", id);

            var result = await _popupService.GetPopupByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Popup bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Silinecek kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogWarning("Popup silme isteği alındı. Id: {Id}", id);

            var result = await _popupService.DeletePopupAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Popup silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Silme işlemi başarısız";
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Popup başarıyla silindi. Id: {Id}", id);
            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}