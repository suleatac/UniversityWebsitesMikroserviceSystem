using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.SitePersonel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class SitePersonelController : Controller
    {
        private readonly ISitePersonelService _sitePersonelService;
        private readonly IUnvanService _unvanService;
        private readonly IPersonelTipService _personelTipService;
        private readonly ILogger<SitePersonelController> _logger;

        public SitePersonelController(
            ISitePersonelService sitePersonelService,
            IUnvanService unvanService,
            IPersonelTipService personelTipService,
            ILogger<SitePersonelController> logger)
        {
            _sitePersonelService = sitePersonelService;
            _unvanService = unvanService;
            _personelTipService = personelTipService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Site Personel listesi getiriliyor.");

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;

            var result = await _sitePersonelService.GetSitePersonellerAsync(currentSiteId);

            if (!result.IsSuccess)
            {
                _logger.LogError("Site Personel listesi alınamadı. Hata: {Error}", result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Personel listesi alınamadı.";
                return View(new List<GetSitePersonelVm>());
            }

            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Site Personel oluşturma sayfası açıldı.");

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;

            var unvanlar = await _unvanService.GetUnvansAsync();
            var personelTipler = await _personelTipService.GetPersonelTiplerAsync();

            var viewModel = new SitePersonelCreateIndexVm
            {
                CreateSitePersonel = new CreateSitePersonelVm { SiteId = currentSiteId },
                Unvanlar = unvanlar.Data ?? new List<ViewModels.Unvan.GetUnvanVm>(),
                PersonelTipler = personelTipler.Data ?? new List<ViewModels.PersonelTip.GetPersonelTipVm>(),
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SitePersonelCreateIndexVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create SitePersonel - ModelState geçersiz.");
                var unvanlar = await _unvanService.GetUnvansAsync();
                var personelTipler = await _personelTipService.GetPersonelTiplerAsync();
                model.Unvanlar = unvanlar.Data ?? new List<ViewModels.Unvan.GetUnvanVm>();
                model.PersonelTipler = personelTipler.Data ?? new List<ViewModels.PersonelTip.GetPersonelTipVm>();
                return View(model);
            }

            var result = await _sitePersonelService.CreateSitePersonelAsync(model.CreateSitePersonel);

            if (!result.IsSuccess)
            {
                _logger.LogError("Site Personel oluşturulamadı. Hata: {Error}", result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Personel oluşturulamadı.");
                var unvanlar = await _unvanService.GetUnvansAsync();
                var personelTipler = await _personelTipService.GetPersonelTiplerAsync();
                model.Unvanlar = unvanlar.Data ?? new List<ViewModels.Unvan.GetUnvanVm>();
                model.PersonelTipler = personelTipler.Data ?? new List<ViewModels.PersonelTip.GetPersonelTipVm>();
                return View(model);
            }

            _logger.LogInformation("Site Personel oluşturuldu.");
            TempData["Success"] = "Personel başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Site Personel düzenleme sayfası açıldı. Id: {Id}", id);

            var result = await _sitePersonelService.GetSitePersonelByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Site Personel bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SitePersonelDetailVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update SitePersonel - ModelState geçersiz.");
                return View(model);
            }

            var result = await _sitePersonelService.UpdateSitePersonelAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError("Site Personel güncellenemedi. Id: {Id}, Hata: {Error}", model.Id, result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");
                return View(model);
            }

            _logger.LogInformation("Site Personel güncellendi. Id: {Id}", model.Id);
            TempData["Success"] = "Personel başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Site Personel delete sayfası açıldı. Id: {Id}", id);

            var result = await _sitePersonelService.GetSitePersonelByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Site Personel bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Silinecek kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogWarning("Site Personel silme isteği alındı. Id: {Id}", id);

            var result = await _sitePersonelService.DeleteSitePersonelAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Site Personel silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Silme işlemi başarısız";
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Site Personel başarıyla silindi. Id: {Id}", id);
            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}