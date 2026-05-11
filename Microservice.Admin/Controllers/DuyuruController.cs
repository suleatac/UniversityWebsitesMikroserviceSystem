using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Duyuru;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class DuyuruController : Controller
    {
        private readonly IDuyuruService _duyuruService;
        private readonly ISiteService _siteService;
        private readonly IDilService _dilService;
        private readonly IHedefService _hedefService;
        private readonly ILogger<DuyuruController> _logger;

        public DuyuruController(
            IDuyuruService duyuruService,
            ISiteService siteService,
            IDilService dilService,
            IHedefService hedefService,
            ILogger<DuyuruController> logger)
        {
            _duyuruService = duyuruService;
            _siteService = siteService;
            _dilService = dilService;
            _hedefService = hedefService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDuyurularForPagination(
            int page = 1, int pageSize = 10, string search = "", int orderColumn = 0, string orderDir = "desc")
        {
            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var columnName = orderColumn switch
            {
                1 => "Baslik",
                2 => "KisaAciklama",
                3 => "YayimTarihi",
                _ => "Id"
            };

            var result = await _duyuruService.GetDuyurularPaginatedAsync(currentSiteId, currentDilId, page, pageSize, search, columnName, orderDir);

            if (!result.IsSuccess)
            {
                _logger.LogError("Paginated duyuru listesi alınamadı. Hata: {Error}", result.Fail?.Detail);
                return BadRequest(new { error = result.Fail?.Detail });
            }

            return Ok(new
            {
                data = result.Data!.Data,
                recordsTotal = result.Data.TotalCount,
                recordsFiltered = result.Data.TotalCount
            });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Duyuru oluşturma sayfası açıldı.");

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var hedefler = await _hedefService.GetHedefsAsync();

            var viewModel = new DuyuruCreateIndexVm
            {
                CreateDuyuru = new CreateDuyuruVm { SiteId = currentSiteId, DilId = currentDilId },
                Hedefler = hedefler.Data ?? new List<ViewModels.Hedef.GetHedefVm>(),
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DuyuruCreateIndexVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create Duyuru - ModelState geçersiz.");
                var hedefler = await _hedefService.GetHedefsAsync();
                model.Hedefler = hedefler.Data ?? new List<ViewModels.Hedef.GetHedefVm>();
                return View(model);
            }

            var result = await _duyuruService.CreateDuyuruAsync(model.CreateDuyuru);

            if (!result.IsSuccess)
            {
                _logger.LogError("Duyuru oluşturulamadı. Hata: {Error}", result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Duyuru oluşturulamadı.");
                var hedefler = await _hedefService.GetHedefsAsync();
                model.Hedefler = hedefler.Data ?? new List<ViewModels.Hedef.GetHedefVm>();
                return View(model);
            }

            _logger.LogInformation("Duyuru oluşturuldu. Başlık: {Title}", model.CreateDuyuru.Baslik);
            TempData["Success"] = "Duyuru başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Duyuru düzenleme sayfası açıldı. Id: {Id}", id);

            var result = await _duyuruService.GetDuyuruByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Duyuru bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DuyuruDetailVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update Duyuru - ModelState geçersiz.");
                return View(model);
            }

            var result = await _duyuruService.UpdateDuyuruAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("Duyuru güncellenemedi. Id: {Id}, Hata: {Error}", model.Id, result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");
                return View(model);
            }

            _logger.LogInformation("Duyuru güncellendi. Id: {Id}", model.Id);
            TempData["Success"] = "Duyuru başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Duyuru delete sayfası açıldı. Id: {Id}", id);

            var result = await _duyuruService.GetDuyuruByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Duyuru bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Silinecek kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogWarning("Duyuru silme isteği alındı. Id: {Id}", id);

            var result = await _duyuruService.DeleteDuyuruAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Duyuru silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Silme işlemi başarısız";
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Duyuru başarıyla silindi. Id: {Id}", id);
            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}