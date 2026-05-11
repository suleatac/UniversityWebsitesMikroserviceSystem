using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Banner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class BannerController : Controller
    {
        private readonly IBannerService _bannerService;
        private readonly ISiteService _siteService;
        private readonly IDilService _dilService;
        private readonly IHedefService _hedefService;
        private readonly ILogger<BannerController> _logger;

        public BannerController(
            IBannerService bannerService,
            ISiteService siteService,
            IDilService dilService,
            IHedefService hedefService,
            ILogger<BannerController> logger)
        {
            _bannerService = bannerService;
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
        public async Task<IActionResult> GetBannersForPagination(
            int page = 1, int pageSize = 10, string search = "", int orderColumn = 0, string orderDir = "desc")
        {
            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var columnName = orderColumn switch
            {
                1 => "Baslik",
                2 => "KisaAciklama",
                3 => "Sira",
                4 => "YayimTarihi",
                _ => "Id"
            };

            var result = await _bannerService.GetBannersPaginatedAsync(currentSiteId, currentDilId, page, pageSize, search, columnName, orderDir);

            if (!result.IsSuccess)
            {
                _logger.LogError("Paginated banner listesi alınamadı. Hata: {Error}", result.Fail?.Detail);
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
            _logger.LogInformation("Banner oluşturma sayfası açıldı.");

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var hedefler = await _hedefService.GetHedefsAsync();

            var viewModel = new BannerCreateIndexVm
            {
                CreateBanner = new CreateBannerVm { SiteId = currentSiteId, DilId = currentDilId },
                Hedefler = hedefler.Data ?? new List<ViewModels.Hedef.GetHedefVm>(),
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BannerCreateIndexVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create Banner - ModelState geçersiz.");
                var hedefler = await _hedefService.GetHedefsAsync();
                model.Hedefler = hedefler.Data ?? new List<ViewModels.Hedef.GetHedefVm>();
                return View(model);
            }

            var result = await _bannerService.CreateBannerAsync(model.CreateBanner);

            if (!result.IsSuccess)
            {
                _logger.LogError("Banner oluşturulamadı. Hata: {Error}", result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Banner oluşturulamadı.");
                var hedefler = await _hedefService.GetHedefsAsync();
                model.Hedefler = hedefler.Data ?? new List<ViewModels.Hedef.GetHedefVm>();
                return View(model);
            }

            _logger.LogInformation("Banner oluşturuldu. Başlık: {Title}", model.CreateBanner.Baslik);
            TempData["Success"] = "Banner başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Banner düzenleme sayfası açıldı. Id: {Id}", id);

            var result = await _bannerService.GetBannerByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Banner bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BannerDetailVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update Banner - ModelState geçersiz.");
                return View(model);
            }

            var result = await _bannerService.UpdateBannerAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("Banner güncellenemedi. Id: {Id}, Hata: {Error}", model.Id, result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");
                return View(model);
            }

            _logger.LogInformation("Banner güncellendi. Id: {Id}", model.Id);
            TempData["Success"] = "Banner başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Banner delete sayfası açıldı. Id: {Id}", id);

            var result = await _bannerService.GetBannerByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Banner bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Silinecek kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogWarning("Banner silme isteği alındı. Id: {Id}", id);

            var result = await _bannerService.DeleteBannerAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Banner silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Silme işlemi başarısız";
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Banner başarıyla silindi. Id: {Id}", id);
            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}