using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.PersonelTip;
using Microservice.Admin.ViewModels.Unvan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class UnvanController : Controller
    {
        private readonly IUnvanService _unvanService;
        private readonly ILogger<UnvanController> _logger;
        private readonly IPersonelTipService _personelTipService;

        public UnvanController(IUnvanService unvanService, ILogger<UnvanController> logger, IPersonelTipService personelTipService)
        {
            _unvanService = unvanService;
            _logger = logger;
            _personelTipService = personelTipService;
        }

        // 🔹 LIST
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Unvan listesi getiriliyor.");

            var result = await _unvanService.GetUnvansAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError("Unvan listesi alınamadı. Hata: {Error}", result.Fail?.Detail);

                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Unvan listesi alınamadı.";
                return View("Error");
            }

            _logger.LogInformation("Unvan listesi başarıyla getirildi. Count: {Count}", result.Data!.Count);

            return View(result.Data);
        }

        // 🔹 CREATE - GET
        [HttpGet]
        public async Task<IActionResult> Create(int? parentId)
        {
            _logger.LogInformation("Unvan oluşturma sayfası açıldı. ParentId: {ParentId}", parentId);

            var unvanlarResult = await _unvanService.GetUnvansAsync();
            var personelTipleriResult = await _personelTipService.GetPersonelTiplerAsync();
            var vm = new UnvanIndexVm
            {
                PersonelTipleri= personelTipleriResult.IsSuccess ? personelTipleriResult.Data! : new List<GetPersonelTipVm>(),
                Unvan = new UnvanVm { ParentId = parentId },
                Unvanlar = unvanlarResult.IsSuccess ? unvanlarResult.Data! : new List<GetUnvanVm>()
            };

            return View(vm);
        }

        // 🔹 CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UnvanIndexVm model)
        {
            // Sira alanı formdan gelmiyor, otomatik hesaplanacak
            ModelState.Remove("Unvan.Sira");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create Unvan - ModelState geçersiz.");

                var unvanlarResult = await _unvanService.GetUnvansAsync();
                var personelTipleriResult = await _personelTipService.GetPersonelTiplerAsync();

                model.Unvanlar = unvanlarResult.IsSuccess ? unvanlarResult.Data! : new List<GetUnvanVm>();
                model.PersonelTipleri = personelTipleriResult.IsSuccess ? personelTipleriResult.Data! : new List<GetPersonelTipVm>();

                return View(model);
            }

            // Aynı parent altındaki maksimum Sira'yı bul ve +1 ata
            var allUnvanlar = await _unvanService.GetUnvansAsync();
            var flatUnvanlar = FlattenUnvanlarList(allUnvanlar.IsSuccess ? allUnvanlar.Data! : new List<GetUnvanVm>());
            var siblings = flatUnvanlar.Where(u => u.ParentId == model.Unvan.ParentId).ToList();
            model.Unvan.Sira = siblings.Any() ? siblings.Max(u => u.Sira) + 1 : 1;

            var result = await _unvanService.CreateUnvanAsync(model.Unvan);

            if (!result.IsSuccess)
            {
                _logger.LogError("Unvan oluşturulamadı. Hata: {Error}", result.Fail?.Detail);

                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Unvan oluşturulamadı.");

                var unvanlarResult = await _unvanService.GetUnvansAsync();
                var personelTipleriResult = await _personelTipService.GetPersonelTiplerAsync();
                model.Unvanlar = unvanlarResult.IsSuccess ? unvanlarResult.Data! : new List<GetUnvanVm>();
                model.PersonelTipleri = personelTipleriResult.IsSuccess ? personelTipleriResult.Data! : new List<GetPersonelTipVm>();
                return View(model);
            }

            _logger.LogInformation("Unvan oluşturuldu.");

            TempData["Success"] = "Unvan başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 UPDATE - GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Unvan edit sayfası açıldı. Id: {Id}", id);

            var result = await _unvanService.GetUnvanByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Unvan bulunamadı. Id: {Id}", id);

                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var updateVm = new UnvanVm
            {
                Id = result.Data.Id,
                TipId = result.Data.TipId,
                Ad = result.Data.Ad,
                KisaAd = result.Data.KisaAd,
                Sira = result.Data.Sira,
                ParentId = result.Data.ParentId
            };

            var unvanlarResult = await _unvanService.GetUnvansAsync();
            var personelTipleriResult = await _personelTipService.GetPersonelTiplerAsync();
            var vm = new UnvanIndexVm
            {
                Unvan = updateVm,
                Unvanlar = unvanlarResult.IsSuccess ? unvanlarResult.Data! : new List<GetUnvanVm>(),
                PersonelTipleri = personelTipleriResult.IsSuccess ? personelTipleriResult.Data! : new List<GetPersonelTipVm>()
            };

            return View(vm);
        }

        // 🔹 UPDATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UnvanIndexVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update Unvan - ModelState geçersiz.");

                var unvanlarResult = await _unvanService.GetUnvansAsync();
                var personelTipleriResult = await _personelTipService.GetPersonelTiplerAsync();
                model.Unvanlar = unvanlarResult.IsSuccess ? unvanlarResult.Data! : new List<GetUnvanVm>();
                model.PersonelTipleri = personelTipleriResult.IsSuccess ? personelTipleriResult.Data! : new List<GetPersonelTipVm>();
                return View(model);
            }

            var result = await _unvanService.UpdateUnvanAsync(model.Unvan);

            if (!result.IsSuccess)
            {
                _logger.LogError("Unvan güncellenemedi. Id: {Id}, Hata: {Error}", model.Unvan.Id, result.Fail?.Detail);

                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");

                var unvanlarResult = await _unvanService.GetUnvansAsync();
                var personelTipleriResult = await _personelTipService.GetPersonelTiplerAsync();

                model.Unvanlar = unvanlarResult.IsSuccess ? unvanlarResult.Data! : new List<GetUnvanVm>();
                model.PersonelTipleri = personelTipleriResult.IsSuccess ? personelTipleriResult.Data! : new List<GetPersonelTipVm>();
                return View(model);
            }

            _logger.LogInformation("Unvan güncellendi. Id: {Id}", model.Unvan.Id);

            TempData["Success"] = "Unvan başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 DELETE - GET
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Unvan delete sayfası açıldı. Id: {Id}", id);

            var result = await _unvanService.GetUnvanByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Unvan bulunamadı. Id: {Id}", id);

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
            _logger.LogWarning("Unvan silme isteği alındı. Id: {Id}", id);

            var result = await _unvanService.DeleteUnvanAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Unvan silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);

                TempData["Error"] = result.Fail?.Detail
                                    ?? result.Fail?.Title
                                    ?? "Silme işlemi başarısız";

                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Unvan başarıyla silindi. Id: {Id}", id);

            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 MOVE - AJAX (Drag & Drop ağaç sıralama)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoveUnvan([FromBody] MoveUnvanVm model)
        {
            if (model == null || model.Id <= 0)
            {
                return Json(new { success = false, message = "Geçersiz veri." });
            }

            _logger.LogInformation("Unvan taşınıyor. Id: {Id}, ParentId: {ParentId}, Sira: {Sira}", model.Id, model.ParentId, model.Sira);

            // Önce mevcut unvan verisini al
            var existingResult = await _unvanService.GetUnvanByIdAsync(model.Id);
            if (!existingResult.IsSuccess || existingResult.Data == null)
            {
                return Json(new { success = false, message = "Unvan bulunamadı." });
            }

            var existing = existingResult.Data;

            // Taşınan unvanı güncelle
            var updateVm = new UnvanVm
            {
                Id = model.Id,
                ParentId = model.ParentId,
                Sira = model.Sira,
                Ad = existing.Ad,
                KisaAd = existing.KisaAd,
                TipId = existing.TipId
            };

            var result = await _unvanService.UpdateUnvanAsync(updateVm);

            if (!result.IsSuccess)
            {
                _logger.LogError("Unvan taşınamadı. Id: {Id}, Hata: {Error}", model.Id, result.Fail?.Detail);
                return Json(new { success = false, message = result.Fail?.Detail ?? "Taşıma işlemi başarısız" });
            }

            // Tüm kardeşlerin Sira değerlerini yeni pozisyonlarına göre güncelle
            if (model.SiblingIds != null && model.SiblingIds.Count > 0)
            {
                for (int i = 0; i < model.SiblingIds.Count; i++)
                {
                    var siblingId = model.SiblingIds[i];
                    if (siblingId == model.Id) continue; // Zaten güncellendi

                    var siblingResult = await _unvanService.GetUnvanByIdAsync(siblingId);
                    if (siblingResult.IsSuccess && siblingResult.Data != null)
                    {
                        var siblingUpdateVm = new UnvanVm
                        {
                            Id = siblingId,
                            ParentId = model.ParentId,
                            Sira = i,
                            Ad = siblingResult.Data.Ad,
                            KisaAd = siblingResult.Data.KisaAd,
                            TipId = siblingResult.Data.TipId
                        };
                        await _unvanService.UpdateUnvanAsync(siblingUpdateVm);
                    }
                }
            }

            _logger.LogInformation("Unvan başarıyla taşındı. Id: {Id}", model.Id);
            return Json(new { success = true, message = "Unvan başarıyla taşındı." });
        }

        // 🔹 jsTree JSON DATA - AJAX
        [HttpGet]
        public async Task<IActionResult> GetUnvanTreeData()
        {
            var result = await _unvanService.GetUnvansAsync();

            if (!result.IsSuccess || result.Data == null)
            {
                return Json(new List<object>());
            }

            var treeData = BuildJsTreeData(result.Data);
            return Json(treeData);
        }

        private List<object> BuildJsTreeData(List<GetUnvanVm> unvanlar, int? parentId = null)
        {
            var items = unvanlar
                .Where(u => u.ParentId == parentId)
                .OrderBy(u => u.Sira)
                .Select(u => new Dictionary<string, object>
                {
                    { "id", u.Id },
                    { "text", u.Ad },
                    { "icon", "mdi mdi-badge-account" },
                    { "children", u.Children != null && u.Children.Any() ? BuildJsTreeData(u.Children.ToList(), u.Id) : new List<object>() },
                    { "data", new { sira = u.Sira, parentId = u.ParentId } },
                    { "a_attr", new { href = $"/Unvan/Edit/{u.Id}" } }
                })
                .ToList<object>();

            return items;
        }

        private List<GetUnvanVm> FlattenUnvanlarList(List<GetUnvanVm> unvanlar)
        {
            var result = new List<GetUnvanVm>();
            foreach (var u in unvanlar)
            {
                result.Add(u);
                if (u.Children != null && u.Children.Any())
                {
                    result.AddRange(FlattenUnvanlarList(u.Children));
                }
            }
            return result;
        }
    }
}
