using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.PersonelTip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PersonelTipController : Controller
    {
        private readonly IPersonelTipService _personelTipService;
        private readonly ILogger<PersonelTipController> _logger;

        public PersonelTipController(IPersonelTipService personelTipService, ILogger<PersonelTipController> logger)
        {
            _personelTipService = personelTipService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Personel tip listesi getiriliyor.");

            var result = await _personelTipService.GetPersonelTiplerAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError("Personel tip listesi alınamadı. Hata: {Error}", result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Personel tip listesi alınamadı.";
                return View("Error");
            }

            _logger.LogInformation("Personel tip listesi başarıyla getirildi. Count: {Count}", result.Data!.Count);
            return View(result.Data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            _logger.LogInformation("Personel tip oluşturma sayfası açıldı.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonelTipVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create PersonelTip - ModelState geçersiz.");
                return View(model);
            }

            var result = await _personelTipService.CreatePersonelTipAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("Personel tip oluşturulamadı. Hata: {Error}", result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Personel tip oluşturulamadı.");
                return View(model);
            }

            _logger.LogInformation("Personel tip oluşturuldu.");
            TempData["Success"] = "Personel tip başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Personel tip düzenleme sayfası açıldı. Id: {Id}", id);

            var result = await _personelTipService.GetPersonelTipByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Personel tip bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PersonelTipVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update PersonelTip - ModelState geçersiz. Id: {Id}", model.Id);
                return View(model);
            }

            var result = await _personelTipService.UpdatePersonelTipAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("Personel tip güncellenemedi. Id: {Id}, Hata: {Error}", model.Id, result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");
                return View(model);
            }

            _logger.LogInformation("Personel tip güncellendi. Id: {Id}", model.Id);
            TempData["Success"] = "Personel tip başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Personel tip delete sayfası açıldı. Id: {Id}", id);

            var result = await _personelTipService.GetPersonelTipByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Personel tip bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Silinecek kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogWarning("Personel tip silme isteği alındı. Id: {Id}", id);

            var result = await _personelTipService.DeletePersonelTipAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Personel tip silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Silme işlemi başarısız";
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Personel tip başarıyla silindi. Id: {Id}", id);
            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}