using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.Template;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TemplateController : Controller
    {
        private readonly ITemplateService _templateService;
        private readonly ILogger<TemplateController> _logger;

        public TemplateController(ITemplateService templateService, ILogger<TemplateController> logger)
        {
            _templateService = templateService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Template listesi getiriliyor.");

            var result = await _templateService.GetTemplatesAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError("Template listesi alınamadı. Hata: {Error}", result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Template listesi alınamadı.";
                return View(new List<GetTemplateVm>());
            }

            _logger.LogInformation("Template listesi başarıyla getirildi. Count: {Count}", result.Data!.Count);
            return View(result.Data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            _logger.LogInformation("Template oluşturma sayfası açıldı.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTemplateVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create Template - ModelState geçersiz.");
                return View(model);
            }

            var result = await _templateService.CreateTemplateAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("Template oluşturulamadı. Hata: {Error}", result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Template oluşturulamadı.");
                return View(model);
            }

            _logger.LogInformation("Template oluşturuldu.");
            TempData["Success"] = "Template başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Template düzenleme sayfası açıldı. Id: {Id}", id);

            var result = await _templateService.GetTemplateByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Template bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var updateModel = new UpdateTemplateVm
            {
                Id = result.Data.Id,
                TemplateAdi = result.Data.TemplateAdi,
                TemplateTuru = result.Data.TemplateTuru,
                FolderName = result.Data.FolderName,
                LayoutPath = result.Data.LayoutPath
            };

            return View(updateModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateTemplateVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update Template - ModelState geçersiz. Id: {Id}", model.Id);
                return View(model);
            }

            var result = await _templateService.UpdateTemplateAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("Template güncellenemedi. Id: {Id}, Hata: {Error}", model.Id, result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");
                return View(model);
            }

            _logger.LogInformation("Template güncellendi. Id: {Id}", model.Id);
            TempData["Success"] = "Template başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Template delete sayfası açıldı. Id: {Id}", id);

            var result = await _templateService.GetTemplateByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Template bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Silinecek kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogWarning("Template silme isteği alındı. Id: {Id}", id);

            var result = await _templateService.DeleteTemplateAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Template silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Silme işlemi başarısız";
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Template başarıyla silindi. Id: {Id}", id);
            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
