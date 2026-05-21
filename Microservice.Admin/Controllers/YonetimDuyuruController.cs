using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.YonetimDuyuru;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class YonetimDuyuruController : Controller
    {
        private readonly IYonetimDuyuruService _yonetimDuyuruService;
        private readonly ILogger<YonetimDuyuruController> _logger;

        public YonetimDuyuruController(IYonetimDuyuruService yonetimDuyuruService, ILogger<YonetimDuyuruController> logger)
        {
            _yonetimDuyuruService = yonetimDuyuruService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetYonetimDuyuruForPagination
           (
              int page = 1,
              int pageSize = 10,
              string search = "",
              int orderColumn = 0,
              string orderDir = "desc"
           )
        {
            // Sütun indeksini isimlere çevir
            var columnName = orderColumn switch
            {
                1 => "Baslik",
                2 => "Tarih",
                _ => "Id"
            };

            var result = await _yonetimDuyuruService.GetYonetimDuyuruPaginatedAsync(page, pageSize, search, columnName, orderDir);

            if (!result.IsSuccess)
            {
                _logger.LogError("Paginated yonetim duyuru listesi alınamadı. Hata: {Error}", result.Fail?.Detail);
                return BadRequest(new { error = result.Fail?.Detail });
            }

            return Ok(new
            {
                data = result.Data!.Data,           // ← İlk Data: ServiceResult, ikinci Data: PaginatedResult.Data
                recordsTotal = result.Data.TotalCount,
                recordsFiltered = result.Data.TotalCount
            });
        }



        // CREATE - GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(YonetimDuyuruVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create YonetimDuyuru - ModelState geçersiz.");
                return View(model);
            }

            var result = await _yonetimDuyuruService.CreateYonetimDuyuruAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("Yonetim duyuru oluşturulamadı. Hata: {Error}", result.Fail?.Detail);

                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Bir hata oluştu");
                return View(model);
            }

            _logger.LogInformation("Yeni duyuru oluşturuldu. DuyuruId: {DuyuruId}", result.Data);
            return RedirectToAction(nameof(Index));
        }

        // UPDATE - GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Yonetim duyuru düzenleme sayfası açılıyor. Id: {Id}", id);

            var yonetimDuyuruResult = await _yonetimDuyuruService.GetYonetimDuyuruByIdAsync(id);

            if (!yonetimDuyuruResult.IsSuccess)
            {
                _logger.LogError("Yonetim duyuru bulunamadı. Id: {Id}", id);
                return NotFound();
            }

            return View(yonetimDuyuruResult.Data!);
        }

        // UPDATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(YonetimDuyuruVm model)
        {


            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update YonetimDuyuru - ModelState geçersiz. Id: {Id}", model.Id);
                return View(model);
            }


            var result = await _yonetimDuyuruService.UpdateYonetimDuyuruAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("Yonetim duyuru güncellenemedi. Id: {Id}, Hata: {Error}", model.Id, result.Fail?.Title);

                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme hatası");
                return View(model);
            }

            _logger.LogInformation("Yonetim duyuru güncellendi. Id: {Id}", model.Id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Yonetim duyuru delete sayfası açıldı. Id: {Id}", id);

            var result = await _yonetimDuyuruService.GetYonetimDuyuruByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Yonetim duyuru bulunamadı. Id: {Id}", id);

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
            _logger.LogWarning("Yonetim duyuru silme isteği alındı. Id: {Id}", id);

            var result = await _yonetimDuyuruService.DeleteYonetimDuyuruAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Yonetim duyuru silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);

                TempData["Error"] = result.Fail?.Detail
                                    ?? result.Fail?.Title
                                    ?? "Silme işlemi başarısız";

                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Yonetim duyuru başarıyla silindi. Id: {Id}", id);

            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }







        // GET DETAIL - AJAX
        [HttpGet]
        public async Task<IActionResult> GetDetail(int id)
        {
            _logger.LogInformation("Yonetim duyuru detaji getiriliyor (AJAX). Id: {Id}", id);

            var result = await _yonetimDuyuruService.GetYonetimDuyuruDetailAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Yonetim duyuru detaji alinamadi. Id: {Id}", id);
                return NotFound(new { error = result.Fail?.Detail ?? "Duyuru detayi bulunamadi" });
            }

            return Ok(result.Data);
        }

        // MARK AS READ - AJAX
        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            _logger.LogInformation("Yonetim duyuru okundu olarak isaretleniyor (AJAX). Id: {Id}", id);

            var result = await _yonetimDuyuruService.MarkYonetimDuyuruAsReadAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Yonetim duyuru okundu isaretlenemedi. Id: {Id}", id);
                return BadRequest(new { error = result.Fail?.Detail ?? "Islem basarisiz" });
            }

            return Ok(new { success = true });
        }
    }
}
