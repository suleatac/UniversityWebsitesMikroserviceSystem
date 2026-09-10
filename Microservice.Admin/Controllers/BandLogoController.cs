using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.BandLogo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class BandLogoController : Controller
    {
        private readonly IBandLogoService _bandLogoService;
        private readonly ILogger<BandLogoController> _logger;

        public BandLogoController(
            IBandLogoService bandLogoService,
            ILogger<BandLogoController> logger)
        {
            _bandLogoService = bandLogoService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetBandLogosForPagination(
            int page = 1, int pageSize = 10, string search = "", int orderColumn = 0, string orderDir = "desc")
        {
            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var columnName = orderColumn switch
            {
                1 => "Ad",
                2 => "EklenmeTarihi",
                _ => "Id"
            };

            var result = await _bandLogoService.GetBandLogosPaginatedAsync(currentSiteId, currentDilId, page, pageSize, search, columnName, orderDir);

            if (!result.IsSuccess)
            {
                _logger.LogError("Paginated BandLogo listesi alınamadı. Hata: {Error}", result.Fail?.Detail);
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
        public IActionResult Create()
        {
            _logger.LogInformation("BandLogo oluşturma sayfası açıldı.");

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var viewModel = new BandLogoCreateIndexVm
            {
                CreateBandLogo = new CreateBandLogoVm { SiteId = currentSiteId, DilId = currentDilId }
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BandLogoCreateIndexVm model)
        {
            // SiteId ve DilId her zaman session'dan gelmelidir (form hidden alanı manipüle edilebilir)
            model.CreateBandLogo.SiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            model.CreateBandLogo.DilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create BandLogo - ModelState geçersiz.");
                return View(model);
            }

            var result = await _bandLogoService.CreateBandLogoAsync(model.CreateBandLogo);

            if (!result.IsSuccess)
            {
                _logger.LogError("BandLogo oluşturulamadı. Hata: {Error}", result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "BandLogo oluşturulamadı.");
                return View(model);
            }

            _logger.LogInformation("BandLogo oluşturuldu. Ad: {Ad}", model.CreateBandLogo.Ad);
            TempData["Success"] = "Band logosu başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("BandLogo düzenleme sayfası açıldı. Id: {Id}", id);

            var result = await _bandLogoService.GetBandLogoByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("BandLogo bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new BandLogoEditIndexVm
            {
                BandLogoDetail = result.Data
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BandLogoEditIndexVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update BandLogo - ModelState geçersiz.");
                return View(model);
            }

            var result = await _bandLogoService.UpdateBandLogoAsync(model.BandLogoDetail);

            if (!result.IsSuccess)
            {
                _logger.LogError("BandLogo güncellenemedi. Id: {Id}, Hata: {Error}", model.BandLogoDetail.Id, result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");
                return View(model);
            }

            _logger.LogInformation("BandLogo güncellendi. Id: {Id}", model.BandLogoDetail.Id);
            TempData["Success"] = "Band logosu başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("BandLogo delete sayfası açıldı. Id: {Id}", id);

            var result = await _bandLogoService.GetBandLogoByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("BandLogo bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Silinecek kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogWarning("BandLogo silme isteği alındı. Id: {Id}", id);

            var result = await _bandLogoService.DeleteBandLogoAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("BandLogo silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Silme işlemi başarısız";
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("BandLogo başarıyla silindi. Id: {Id}", id);
            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
