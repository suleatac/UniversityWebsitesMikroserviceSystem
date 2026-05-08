using Microservice.Admin.Services;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.Haber;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class HaberController : Controller
    {
        private readonly IHaberService _haberService;
        private readonly ISiteService _siteService;
        private readonly IDilService _dilService;
        private readonly IHedefService _hedefService;
        private readonly ILogger<HaberController> _logger;

        public HaberController(
            IHaberService haberService,
            ISiteService siteService,
            IDilService dilService,
            IHedefService hedefService,
            ILogger<HaberController> logger)
        {
            _haberService = haberService;
            _siteService = siteService;
            _dilService = dilService;
            _hedefService = hedefService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        // 🔹 PAGINATED LIST
        [HttpGet]
        public async Task<IActionResult> GetHabersForPagination
            (
               int page = 1,
               int pageSize = 2,
               string search = "",
               int orderColumn = 0,
               string orderDir = "desc"
            )
        {
            // Session'dan değerleri al, parametre olarak geldiyse onları kullan
            var currentSiteId =  HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId =  HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            // Sütun indeksini isimlere çevir
            var columnName = orderColumn switch {
                1 => "Baslik",
                2 => "KisaAciklama",
                3 => "YayimTarihi",
                _ => "Id"
            };

            var result = await _haberService.GetHabersPaginatedAsync(currentSiteId, currentDilId, page, pageSize, search, columnName, orderDir);

            if (!result.IsSuccess)
            {
                _logger.LogError("Paginated haber listesi alınamadı. Hata: {Error}", result.Fail?.Detail);
                return BadRequest(new { error = result.Fail?.Detail });
            }

            return Ok(new {
                data = result.Data!.Data,
                recordsTotal = result.Data.TotalCount,
                recordsFiltered = result.Data.TotalCount
            });
        }




        // 🔹 DETAIL - GET
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Haber detayı getiriliyor. Id: {Id}", id);

            var result = await _haberService.GetHaberByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Haber bulunamadı. Id: {Id}", id);

                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        // 🔹 CREATE - GET
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Haber oluşturma sayfası açıldı.");

            // Session'dan site ve dil seçimini al
            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var siteler = await _siteService.GetSitesAsync();
            var diller = await _dilService.GetDilsAsync();
            var hedefler = await _hedefService.GetHedefsAsync();

            var viewModel = new HaberCreateIndexVm
            {
                CreateHaber = new CreateHaberVm
                {
                    SiteId = currentSiteId,
                    DilId = currentDilId
                },
                Hedefler = hedefler.Data ?? new List<ViewModels.Hedef.GetHedefVm>(),

            };

            return View(viewModel);
        }

        // 🔹 CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HaberCreateIndexVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create Haber - ModelState geçersiz.");

                // Dropdown'ları yeniden yükle
                var hedefler = await _hedefService.GetHedefsAsync();
                var siteler = await _siteService.GetSitesAsync();
                var diller = await _dilService.GetDilsAsync();
                model.Hedefler = hedefler.Data ?? new List<ViewModels.Hedef.GetHedefVm>();

                return View(model);
            }

            var result = await _haberService.CreateHaberAsync(model.CreateHaber);

            if (!result.IsSuccess)
            {
                _logger.LogError("Haber oluşturulamadı. Hata: {Error}", result.Fail?.Detail);

                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Haber oluşturulamadı.");

                // Dropdown'ları yeniden yükle
                var hedefler = await _hedefService.GetHedefsAsync();
                var siteler = await _siteService.GetSitesAsync();
                var diller = await _dilService.GetDilsAsync();
                model.Hedefler = hedefler.Data ?? new List<ViewModels.Hedef.GetHedefVm>();


                return View(model);
            }

            _logger.LogInformation("Haber oluşturuldu. Başlık: {Title}", model.CreateHaber.Baslik);

            TempData["Success"] = "Haber başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index), new { siteId = model.CreateHaber.SiteId, dilId = model.CreateHaber.DilId });
        }

        // 🔹 UPDATE - GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Haber edit sayfası açıldı. Id: {Id}", id);

            var result = await _haberService.GetHaberByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Haber bulunamadı. Id: {Id}", id);

                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        // 🔹 UPDATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HaberDetailVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update Haber - ModelState geçersiz.");
                return View(model);
            }

            var result = await _haberService.UpdateHaberAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("Haber güncellenemedi. Id: {Id}, Hata: {Error}", model.Id, result.Fail?.Detail);

                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");
                return View(model);
            }

            _logger.LogInformation("Haber güncellendi. Id: {Id}", model.Id);

            TempData["Success"] = "Haber başarıyla güncellendi.";
            return RedirectToAction(nameof(Index), new { siteId = model.SiteId, dilId = model.DilId });
        }

        // 🔹 DELETE - GET
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Haber delete sayfası açıldı. Id: {Id}", id);

            var result = await _haberService.GetHaberByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Haber bulunamadı. Id: {Id}", id);

                TempData["Error"] = "Silinecek kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        // 🔹 DELETE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogWarning("Haber silme isteği alındı. Id: {Id}", id);

            var result = await _haberService.DeleteHaberAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Haber silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);

                TempData["Error"] = result.Fail?.Detail
                                    ?? result.Fail?.Title
                                    ?? "Silme işlemi başarısız";

                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Haber başarıyla silindi. Id: {Id}", id);

            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
