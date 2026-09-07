using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.ShortcutButton;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class ShortcutButtonController : Controller
    {
        private readonly IShortcutButtonService _shortcutButtonService;
        private readonly IDilService _dilService;
        private readonly IHedefService _hedefService;
        private readonly ILogger<ShortcutButtonController> _logger;

        public ShortcutButtonController(
            IShortcutButtonService shortcutButtonService,
            IDilService dilService,
            IHedefService hedefService,
            ILogger<ShortcutButtonController> logger)
        {
            _shortcutButtonService = shortcutButtonService;
            _dilService = dilService;
            _hedefService = hedefService;
            _logger = logger;
        }

        // 🔹 LIST
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("ShortcutButton listesi getiriliyor.");

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var result = await _shortcutButtonService.GetShortcutButtonsAsync(currentSiteId, currentDilId);

            if (!result.IsSuccess)
            {
                _logger.LogError("ShortcutButton listesi alınamadı. Hata: {Error}", result.Fail?.Detail);

                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Kısayol butonları alınamadı.";
                return View("Error");
            }

            _logger.LogInformation("ShortcutButton listesi başarıyla getirildi. Count: {Count}", result.Data!.Count);

            return View(result.Data);
        }

        // 🔹 CREATE - GET
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("ShortcutButton oluşturma sayfası açıldı.");

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var dillerResult = await _dilService.GetDilsAsync();
            var hedeflerResult = await _hedefService.GetHedefsAsync();

            var vm = new ShortcutButtonCreateIndexVm
            {
                CreateShortcutButton = new ShortcutButtonVm
                {
                    SiteId = currentSiteId,
                    DilId = currentDilId,
                    IsIconImage = true
                },
                Diller = dillerResult.IsSuccess ? dillerResult.Data! : new List<ViewModels.Dil.GetDilVm>(),
                Hedefler = hedeflerResult.IsSuccess ? hedeflerResult.Data! : new List<ViewModels.Hedef.GetHedefVm>()
            };

            return View(vm);
        }

        // 🔹 CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShortcutButtonCreateIndexVm model)
        {
            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            // Sira alanı formdan gelmiyor, otomatik hesaplanacak
            ModelState.Remove("CreateShortcutButton.Sira");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create ShortcutButton - ModelState geçersiz.");

                var dillerResult = await _dilService.GetDilsAsync();
                var hedeflerResult = await _hedefService.GetHedefsAsync();

                model.Diller = dillerResult.IsSuccess ? dillerResult.Data! : new List<ViewModels.Dil.GetDilVm>();
                model.Hedefler = hedeflerResult.IsSuccess ? hedeflerResult.Data! : new List<ViewModels.Hedef.GetHedefVm>();
                return View(model);
            }

            // Mevcut kısayol butonlarındaki maksimum Sira'yı bul ve +1 ata
            var existing = await _shortcutButtonService.GetShortcutButtonsAsync(currentSiteId, currentDilId);
            var existingList = existing.IsSuccess && existing.Data != null
                ? existing.Data
                : new List<ViewModels.ShortcutButton.GetShortcutButtonVm>();

            model.CreateShortcutButton.Sira = existingList.Any() ? existingList.Max(b => b.Sira) + 1 : 1;

            // SiteId ve DilId'yi session'dan set et
            model.CreateShortcutButton.SiteId = currentSiteId;
            model.CreateShortcutButton.DilId = currentDilId;

            var result = await _shortcutButtonService.CreateShortcutButtonAsync(model.CreateShortcutButton);

            if (!result.IsSuccess)
            {
                _logger.LogError("ShortcutButton oluşturulamadı. Hata: {Error}", result.Fail?.Detail);

                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Kısayol butonu oluşturulamadı.");

                var dillerResult = await _dilService.GetDilsAsync();
                var hedeflerResult = await _hedefService.GetHedefsAsync();

                model.Diller = dillerResult.IsSuccess ? dillerResult.Data! : new List<ViewModels.Dil.GetDilVm>();
                model.Hedefler = hedeflerResult.IsSuccess ? hedeflerResult.Data! : new List<ViewModels.Hedef.GetHedefVm>();
                return View(model);
            }

            _logger.LogInformation("ShortcutButton oluşturuldu. Ad: {Ad}", model.CreateShortcutButton.Ad);

            TempData["Success"] = "Kısayol butonu başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 UPDATE - GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("ShortcutButton edit sayfası açıldı. Id: {Id}", id);

            var result = await _shortcutButtonService.GetShortcutButtonByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("ShortcutButton bulunamadı. Id: {Id}", id);

                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var dillerResult = await _dilService.GetDilsAsync();
            var hedeflerResult = await _hedefService.GetHedefsAsync();

            var vm = new ShortcutButtonEditIndexVm
            {
                ShortcutButton = result.Data,
                Diller = dillerResult.IsSuccess ? dillerResult.Data! : new List<ViewModels.Dil.GetDilVm>(),
                Hedefler = hedeflerResult.IsSuccess ? hedeflerResult.Data! : new List<ViewModels.Hedef.GetHedefVm>()
            };

            return View(vm);
        }

        // 🔹 UPDATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ShortcutButtonEditIndexVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update ShortcutButton - ModelState geçersiz.");

                var dillerResult = await _dilService.GetDilsAsync();
                var hedeflerResult = await _hedefService.GetHedefsAsync();

                model.Diller = dillerResult.IsSuccess ? dillerResult.Data! : new List<ViewModels.Dil.GetDilVm>();
                model.Hedefler = hedeflerResult.IsSuccess ? hedeflerResult.Data! : new List<ViewModels.Hedef.GetHedefVm>();
                return View(model);
            }

            var result = await _shortcutButtonService.UpdateShortcutButtonAsync(model.ShortcutButton);

            if (!result.IsSuccess)
            {
                _logger.LogError("ShortcutButton güncellenemedi. Id: {Id}, Hata: {Error}", model.ShortcutButton.Id, result.Fail?.Detail);

                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");

                var dillerResult = await _dilService.GetDilsAsync();
                var hedeflerResult = await _hedefService.GetHedefsAsync();

                model.Diller = dillerResult.IsSuccess ? dillerResult.Data! : new List<ViewModels.Dil.GetDilVm>();
                model.Hedefler = hedeflerResult.IsSuccess ? hedeflerResult.Data! : new List<ViewModels.Hedef.GetHedefVm>();
                return View(model);
            }

            _logger.LogInformation("ShortcutButton güncellendi. Id: {Id}", model.ShortcutButton.Id);

            TempData["Success"] = "Kısayol butonu başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 DELETE - GET
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("ShortcutButton delete sayfası açıldı. Id: {Id}", id);

            var result = await _shortcutButtonService.GetShortcutButtonByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("ShortcutButton bulunamadı. Id: {Id}", id);

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
            _logger.LogWarning("ShortcutButton silme isteği alındı. Id: {Id}", id);

            var result = await _shortcutButtonService.DeleteShortcutButtonAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("ShortcutButton silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);

                TempData["Error"] = result.Fail?.Detail
                                    ?? result.Fail?.Title
                                    ?? "Silme işlemi başarısız";

                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("ShortcutButton başarıyla silindi. Id: {Id}", id);

            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
