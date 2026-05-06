using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.Template;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class TemplateController : Controller
    {
        private readonly ITemplateService _templateService;
        private readonly ILogger<TemplateController> _logger;

        public TemplateController(ITemplateService templateService, ILogger<TemplateController> logger)
        {
            _templateService = templateService;
            _logger = logger;
        }

        // 🔹 LIST
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Template listesi getiriliyor.");

            var result = await _templateService.GetTemplatesAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError("Template listesi alınamadı. Hata: {Error}", result.Fail?.Detail);

                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Template listesi alınamadı.";
                return View("Error");
            }

            _logger.LogInformation("Template listesi başarıyla getirildi. Count: {Count}", result.Data!.Count);

            return View(result.Data);
        }

        // 🔹 CREATE - GET
        [HttpGet]
        public IActionResult Create()
        {
            _logger.LogInformation("Template oluşturma sayfası açıldı.");
            return View();
        }

        // 🔹 CREATE - POST
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

            _logger.LogInformation("Template oluşturuldu. Id: {Id}", result);

            TempData["Success"] = "Template başarıyla oluşturuldu.";
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
        // DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogWarning("Template silme isteği alındı. Id: {Id}", id);

            var result = await _templateService.DeleteTemplateAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Template silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);

                TempData["Error"] = result.Fail?.Detail
                                    ?? result.Fail?.Title
                                    ?? "Silme işlemi başarısız";

                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Template başarıyla silindi. Id: {Id}", id);

            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }





    }
}
