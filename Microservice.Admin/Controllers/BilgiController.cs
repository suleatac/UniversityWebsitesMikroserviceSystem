using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Bilgi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class BilgiController : Controller
    {
        private readonly IBilgiService _bilgiService;
        private readonly ISiteService _siteService;
        private readonly IDilService _dilService;
        private readonly IHedefService _hedefService;
        private readonly ILogger<BilgiController> _logger;

        public BilgiController(
            IBilgiService bilgiService,
            ISiteService siteService,
            IDilService dilService,
            IHedefService hedefService,
            ILogger<BilgiController> logger)
        {
            _bilgiService = bilgiService;
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
        public async Task<IActionResult> GetBilgilerForPagination(
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

            var result = await _bilgiService.GetBilgilerPaginatedAsync(currentSiteId, currentDilId, page, pageSize, search, columnName, orderDir);

            if (!result.IsSuccess)
            {
                _logger.LogError("Paginated bilgi listesi alınamadı. Hata: {Error}", result.Fail?.Detail);
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
            _logger.LogInformation("Bilgi oluşturma sayfası açıldı.");

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var hedefler = await _hedefService.GetHedefsAsync();

            var viewModel = new BilgiCreateIndexVm
            {
                CreateBilgi = new CreateBilgiVm { SiteId = currentSiteId, DilId = currentDilId },
                Hedefler = hedefler.Data ?? new List<ViewModels.Hedef.GetHedefVm>(),
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BilgiCreateIndexVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create Bilgi - ModelState geçersiz.");
                var hedefler = await _hedefService.GetHedefsAsync();
                model.Hedefler = hedefler.Data ?? new List<ViewModels.Hedef.GetHedefVm>();
                return View(model);
            }

            var result = await _bilgiService.CreateBilgiAsync(model.CreateBilgi);

            if (!result.IsSuccess)
            {
                _logger.LogError("Bilgi oluşturulamadı. Hata: {Error}", result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Bilgi oluşturulamadı.");
                var hedefler = await _hedefService.GetHedefsAsync();
                model.Hedefler = hedefler.Data ?? new List<ViewModels.Hedef.GetHedefVm>();
                return View(model);
            }

            _logger.LogInformation("Bilgi oluşturuldu. Başlık: {Title}", model.CreateBilgi.Baslik);
            TempData["Success"] = "Bilgi başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Bilgi düzenleme sayfası açıldı. Id: {Id}", id);

            var result = await _bilgiService.GetBilgiByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Bilgi bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BilgiDetailVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update Bilgi - ModelState geçersiz.");
                return View(model);
            }

            var result = await _bilgiService.UpdateBilgiAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("Bilgi güncellenemedi. Id: {Id}, Hata: {Error}", model.Id, result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");
                return View(model);
            }

            _logger.LogInformation("Bilgi güncellendi. Id: {Id}", model.Id);
            TempData["Success"] = "Bilgi başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Bilgi delete sayfası açıldı. Id: {Id}", id);

            var result = await _bilgiService.GetBilgiByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Bilgi bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Silinecek kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogWarning("Bilgi silme isteği alındı. Id: {Id}", id);

            var result = await _bilgiService.DeleteBilgiAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Bilgi silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Silme işlemi başarısız";
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Bilgi başarıyla silindi. Id: {Id}", id);
            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}