using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.GaleriResim;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class GaleriResimController : Controller
    {
        private readonly IGaleriResimService _galeriResimService;
        private readonly IHedefService _hedefService;
        private readonly ILogger<GaleriResimController> _logger;

        public GaleriResimController(
            IGaleriResimService galeriResimService,
            IHedefService hedefService,
            ILogger<GaleriResimController> logger)
        {
            _galeriResimService = galeriResimService;
            _hedefService = hedefService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetGaleriResimlerForPagination(
            int page = 1, int pageSize = 10, string search = "", int orderColumn = 0, string orderDir = "desc")
        {
            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var columnName = orderColumn switch
            {
                1 => "Baslik",
                2 => "KisaAciklama",
                3 => "Kategori",
                4 => "YayimTarihi",
                _ => "Id"
            };

            var result = await _galeriResimService.GetGaleriResimlerPaginatedAsync(currentSiteId, currentDilId, page, pageSize, search, columnName, orderDir);

            if (!result.IsSuccess)
            {
                _logger.LogError("Paginated galeri resim listesi alınamadı. Hata: {Error}", result.Fail?.Detail);
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
            _logger.LogInformation("Galeri resimi oluşturma sayfası açıldı.");

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var hedefler = await _hedefService.GetHedefsAsync();

            var viewModel = new GaleriResimCreateIndexVm
            {
                CreateGaleriResim = new CreateGaleriResimVm { SiteId = currentSiteId, DilId = currentDilId },
                Hedefler = hedefler.Data ?? new List<ViewModels.Hedef.GetHedefVm>(),
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GaleriResimCreateIndexVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create GaleriResim - ModelState geçersiz.");
                var hedefler = await _hedefService.GetHedefsAsync();
                model.Hedefler = hedefler.Data ?? new List<ViewModels.Hedef.GetHedefVm>();
                return View(model);
            }

            var result = await _galeriResimService.CreateGaleriResimAsync(model.CreateGaleriResim);

            if (!result.IsSuccess)
            {
                _logger.LogError("Galeri resimi oluşturulamadı. Hata: {Error}", result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Galeri resimi oluşturulamadı.");
                var hedefler = await _hedefService.GetHedefsAsync();
                model.Hedefler = hedefler.Data ?? new List<ViewModels.Hedef.GetHedefVm>();
                return View(model);
            }

            _logger.LogInformation("Galeri resimi oluşturuldu. Başlık: {Title}", model.CreateGaleriResim.Baslik);
            TempData["Success"] = "Galeri resimi başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Galeri resimi düzenleme sayfası açıldı. Id: {Id}", id);

            var result = await _galeriResimService.GetGaleriResimByIdAsync(id);
            var hedefler = await _hedefService.GetHedefsAsync();

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Galeri resimi bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new GaleriResimEditIndexVm {
                GaleriResimDetail = result.Data,
                Hedefler = hedefler.Data ?? new List<ViewModels.Hedef.GetHedefVm>(),
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GaleriResimEditIndexVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update GaleriResim - ModelState geçersiz.");
                return View(model);
            }

            var result = await _galeriResimService.UpdateGaleriResimAsync(model.GaleriResimDetail);

            if (!result.IsSuccess)
            {
                _logger.LogError("Galeri resimi güncellenemedi. Id: {Id}, Hata: {Error}", model.GaleriResimDetail.Id, result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");
                return View(model);
            }

            _logger.LogInformation("Galeri resimi güncellendi. Id: {Id}", model.GaleriResimDetail.Id);
            TempData["Success"] = "Galeri resimi başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Galeri resimi delete sayfası açıldı. Id: {Id}", id);

            var result = await _galeriResimService.GetGaleriResimByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Galeri resimi bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Silinecek kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogWarning("Galeri resimi silme isteği alındı. Id: {Id}", id);

            var result = await _galeriResimService.DeleteGaleriResimAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Galeri resimi silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Silme işlemi başarısız";
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Galeri resimi başarıyla silindi. Id: {Id}", id);
            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
