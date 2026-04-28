using Microservice.Admin.Services.Interfaces;

namespace Microservice.Admin.Controllers
{
    using Microservice.Admin.ViewModels.Site;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Authorize]
    public class SiteController : Controller
    {
        private readonly ISiteService _siteService;
        private readonly ILogger<SiteController> _logger;

        public SiteController(ISiteService siteService, ILogger<SiteController> logger)
        {
            _siteService = siteService;
            _logger = logger;
        }

        // LIST
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Site listesi getiriliyor.");

            var result = await _siteService.GetSitesAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError("Site listesi alınamadı. Hata: {Error}", result.Fail?.Detail);

                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title;
                return View("Error");
            }

            _logger.LogInformation("Site listesi başarıyla getirildi. Count: {Count}", result.Data!.Count);
            return View(result.Data);
        }

        // CREATE - GET
        [HttpGet]
        public IActionResult Create()
        {
            _logger.LogInformation("Site oluşturma sayfası açıldı.");
            return View();
        }

        // CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSiteVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create Site - ModelState geçersiz.");
                return View(model);
            }

            var result = await _siteService.CreateSiteAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("Site oluşturulamadı. Hata: {Error}", result.Fail?.Detail);

                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Bir hata oluştu");
                return View(model);
            }

            _logger.LogInformation("Yeni site oluşturuldu. SiteId: {SiteId}", result.Data?.Id);
            return RedirectToAction(nameof(Index));
        }

        // UPDATE - GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Site düzenleme sayfası açılıyor. Id: {Id}", id);

            var result = await _siteService.GetSiteByIdAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Site bulunamadı. Id: {Id}", id);
                return NotFound();
            }

            return View(result.Data);
        }

        // UPDATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateSiteVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update Site - ModelState geçersiz. Id: {Id}", model.Id);
                return View(model);
            }

            var result = await _siteService.UpdateSiteAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("Site güncellenemedi. Id: {Id}, Hata: {Error}", model.Id, result.Fail?.Detail);

                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme hatası");
                return View(model);
            }

            _logger.LogInformation("Site güncellendi. Id: {Id}", model.Id);
            return RedirectToAction(nameof(Index));
        }
        // DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogWarning("Site silme isteği alındı. Id: {Id}", id);

            var result = await _siteService.DeleteSiteAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Site silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);

                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Silme işlemi başarısız";
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Site silindi. Id: {Id}", id);
            return RedirectToAction(nameof(Index));
        }
    }
}
