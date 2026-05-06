using Microservice.Admin.Services.Interfaces;

namespace Microservice.Admin.Controllers
{
    using Microservice.Admin.Clients.SiteClients;
    using Microservice.Admin.Services;
    using Microservice.Admin.ViewModels.Site;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

    [Authorize]
    public class SiteController : Controller
    {
        private readonly ISiteService _siteService;
        private readonly ILogger<SiteController> _logger;
        private readonly ITemplateService _templateService;
        private readonly IBirimService _birimService;

        public SiteController
            (
               ISiteService siteService, 
               ILogger<SiteController> logger, 
               ITemplateService templateService, 
               IBirimService birimService
            )
        {
            _siteService = siteService;
            _logger = logger;
            _templateService = templateService;
            _birimService = birimService;
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

        [HttpGet]
        public async Task<IActionResult> GetSitesForPagination
            (
               int page = 1,
               int pageSize = 10,
               string search = "",
               int orderColumn = 0,
               string orderDir = "desc"
            )
        {
            // Sütun indeksini isimlere çevir
            var columnName = orderColumn switch {
                1 => "SiteAdi",
                2 => "SiteUrl",
                3 => "SiteEPosta",
                _ => "Id"
            };

            var result = await _siteService.GetSitesPaginatedAsync(page, pageSize, search, columnName, orderDir);

            if (!result.IsSuccess)
            {
                _logger.LogError("Paginated site listesi alınamadı. Hata: {Error}", result.Fail?.Detail);
                return BadRequest(new { error = result.Fail?.Detail });
            }

            return Ok(new {
                data = result.Data!.Data,           // ← İlk Data: ServiceResult, ikinci Data: PaginatedResult.Data
                recordsTotal = result.Data.TotalCount,
                recordsFiltered = result.Data.TotalCount
            });
        }








        // CREATE - GET
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var birimler = await _birimService.GetBirimsAsync();
            var templates = await _templateService.GetTemplatesAsync();

            var viewModel = new SiteIndexVm {
                CreateSite = new CreateSiteVm(),
                Birimler = birimler.Data!,
                Templates = templates.Data!
            };

            return View(viewModel);
        }

        // CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SiteIndexVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create Site - ModelState geçersiz.");
                return View(model);
            }

            var result = await _siteService.CreateSiteAsync(model.CreateSite);

            if (!result.IsSuccess)
            {
                _logger.LogError("Site oluşturulamadı. Hata: {Error}", result.Fail?.Detail);

                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Bir hata oluştu");
                return View(model);
            }

            _logger.LogInformation("Yeni site oluşturuldu. SiteId: {SiteId}", result.Data);
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
        public async Task<IActionResult> Edit(SiteDetailGetVm model)
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
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Site delete sayfası açıldı. Id: {Id}", id);

            var result = await _siteService.GetSiteByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Site bulunamadı. Id: {Id}", id);

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
            _logger.LogWarning("Site silme isteği alındı. Id: {Id}", id);

            var result = await _siteService.DeleteSiteAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Site silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);

                TempData["Error"] = result.Fail?.Detail
                                    ?? result.Fail?.Title
                                    ?? "Silme işlemi başarısız";

                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Site başarıyla silindi. Id: {Id}", id);

            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }

    }
}
