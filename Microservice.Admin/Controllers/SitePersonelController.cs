using Microservice.Admin.Services;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.SitePersonel;
using Microservice.Admin.ViewModels.TumPersonel;
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
        private readonly ITumPersonelService _tumPersonelService;
        private readonly ILogger<SitePersonelController> _logger;

        public SitePersonelController(
            ISitePersonelService sitePersonelService,
            IUnvanService unvanService,
            IPersonelTipService personelTipService,
            ITumPersonelService tumPersonelService,
            ILogger<SitePersonelController> logger)
        {
            _tumPersonelService = tumPersonelService;
            _sitePersonelService = sitePersonelService;
            _unvanService = unvanService;
            _personelTipService = personelTipService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Site Personel listesi getiriliyor.");

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId");

            if (!currentSiteId.HasValue)
            {
                _logger.LogError("CurrentSiteId session değeri bulunamadı.");

                TempData["Error"] = "Tekrar site seçiniz veya tekrar giriş yapınız.";

                return RedirectToAction("SelectSite", "SiteSelection");
            }

            var result = await _sitePersonelService
                .GetSitePersonellerAsync(currentSiteId.Value);

            if (!result.IsSuccess)
            {
                _logger.LogError(
                    "Site personel listesi alınamadı. Hata: {Error}",
                    result.Fail?.Detail
                );

                TempData["Error"] =
                    result.Fail?.Detail ??
                    result.Fail?.Title ??
                    "Personel listesi alınamadı.";

                return View(new List<GetSitePersonelVm>());
            }

            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Site Personel oluşturma sayfası açıldı.");

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId");

            if (!currentSiteId.HasValue)
            {
                _logger.LogError("CurrentSiteId session değeri bulunamadı.");

                TempData["Error"] = "Tekrar site seçiniz veya tekrar giriş yapınız.";

                return RedirectToAction("SelectSite", "SiteSelection");
            }


            var unvanlar = await _unvanService.GetUnvansAsync();
            var personelTipler = await _personelTipService.GetPersonelTiplerAsync();
            var personelResult = await _tumPersonelService.GetTumPersonelsAsync();

            var viewModel = new SitePersonelCreateIndexVm
            {
                CreateSitePersonel = new CreateSitePersonelVm { SiteId = currentSiteId.Value },
                Unvanlar = unvanlar.Data ?? new List<ViewModels.Unvan.GetUnvanVm>(),
                PersonelTipler = personelTipler.Data ?? new List<ViewModels.PersonelTip.GetPersonelTipVm>(),
                TumPersoneller= personelResult.IsSuccess ? personelResult.Data ?? new List<GetPersonelVm>() : new List<GetPersonelVm>()
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
                var personelResult = await _tumPersonelService.GetTumPersonelsAsync();

                model.Unvanlar = unvanlar.Data ?? new List<ViewModels.Unvan.GetUnvanVm>();
                model.PersonelTipler = personelTipler.Data ?? new List<ViewModels.PersonelTip.GetPersonelTipVm>();
                model.TumPersoneller = personelResult.IsSuccess ? personelResult.Data ?? new List<GetPersonelVm>() : new List<GetPersonelVm>();

                return View(model);
            }

            var result = await _sitePersonelService.CreateSitePersonelAsync(model.CreateSitePersonel);

            if (!result.IsSuccess)
            {
                _logger.LogError("Site Personel oluşturulamadı. Hata: {Error}", result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Personel oluşturulamadı.");

                var unvanlar = await _unvanService.GetUnvansAsync();
                var personelTipler = await _personelTipService.GetPersonelTiplerAsync();
                var personelResult = await _tumPersonelService.GetTumPersonelsAsync();

                model.Unvanlar = unvanlar.Data ?? new List<ViewModels.Unvan.GetUnvanVm>();
                model.PersonelTipler = personelTipler.Data ?? new List<ViewModels.PersonelTip.GetPersonelTipVm>();
                model.TumPersoneller = personelResult.IsSuccess ? personelResult.Data ?? new List<GetPersonelVm>() : new List<GetPersonelVm>();

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

            var unvanlar = await _unvanService.GetUnvansAsync();
            var personelTipler = await _personelTipService.GetPersonelTiplerAsync();
            var personelResult = await _tumPersonelService.GetTumPersonelsAsync();

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Site Personel bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }
            var viewModel = new SitePersonelEditIndexVm {
                EditSitePersonel = result.Data,
                Unvanlar = unvanlar.Data ?? new List<ViewModels.Unvan.GetUnvanVm>(),
                PersonelTipler = personelTipler.Data ?? new List<ViewModels.PersonelTip.GetPersonelTipVm>(),
                TumPersoneller = personelResult.IsSuccess ? personelResult.Data ?? new List<GetPersonelVm>() : new List<GetPersonelVm>()
            };



            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SitePersonelEditIndexVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update SitePersonel - ModelState geçersiz.");
                return View(model);
            }

            var result = await _sitePersonelService.UpdateSitePersonelAsync(model.EditSitePersonel);

            if (!result.IsSuccess)
            {
                _logger.LogError("Site Personel güncellenemedi. Id: {Id}, Hata: {Error}", model.EditSitePersonel.Id, result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");
                return View(model);
            }

            _logger.LogInformation("Site Personel güncellendi. Id: {Id}", model.EditSitePersonel.Id);
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