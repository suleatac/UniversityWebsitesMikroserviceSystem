using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.Birim;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BirimController : Controller
    {
        private readonly IBirimService _birimService;
        private readonly ILogger<BirimController> _logger;

        public BirimController(IBirimService birimService, ILogger<BirimController> logger)
        {
            _birimService = birimService;
            _logger = logger;
        }

        // 🔹 LIST
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Birim listesi getiriliyor.");

            var result = await _birimService.GetBirimsAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError("Birim listesi alınamadı. Hata: {Error}", result.Fail?.Detail);

                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Birim listesi alınamadı.";
                return View("Error");
            }

            _logger.LogInformation("Birim listesi başarıyla getirildi. Count: {Count}", result.Data!.Count);

            return View(result.Data);
        }

        // 🔹 CREATE - GET
        [HttpGet]
        public async Task<IActionResult> Create(int? parentId)
        {
            _logger.LogInformation("Birim oluşturma sayfası açıldı. ParentId: {ParentId}", parentId);

            var birimlerResult = await _birimService.GetBirimsAsync();
            var vm = new BirimCreateIndexVm
            {
                CreateBirim = new CreateBirimVm { ParentId = parentId },
                Birimler = birimlerResult.IsSuccess ? birimlerResult.Data! : new List<GetBirimVm>()
            };

            return View(vm);
        }

        // 🔹 CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BirimCreateIndexVm model)
        {
            // Sira alanı formdan gelmiyor, otomatik hesaplanacak
            ModelState.Remove("CreateBirim.Sira");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create Birim - ModelState geçersiz.");

                var birimlerResult = await _birimService.GetBirimsAsync();
                model.Birimler = birimlerResult.IsSuccess ? birimlerResult.Data! : new List<GetBirimVm>();
                return View(model);
            }

            // Aynı parent altındaki maksimum Sira'yı bul ve +1 ata
            var allBirimler = await _birimService.GetBirimsAsync();
            var flatBirimler = FlattenBirimlerList(allBirimler.IsSuccess ? allBirimler.Data! : new List<GetBirimVm>());
            var siblings = flatBirimler.Where(b => b.ParentId == model.CreateBirim.ParentId).ToList();
            model.CreateBirim.Sira = siblings.Any() ? siblings.Max(b => b.Sira) + 1 : 1;

            var result = await _birimService.CreateBirimAsync(model.CreateBirim);

            if (!result.IsSuccess)
            {
                _logger.LogError("Birim oluşturulamadı. Hata: {Error}", result.Fail?.Detail);

                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Birim oluşturulamadı.");

                var birimlerResult = await _birimService.GetBirimsAsync();
                model.Birimler = birimlerResult.IsSuccess ? birimlerResult.Data! : new List<GetBirimVm>();
                return View(model);
            }

            _logger.LogInformation("Birim oluşturuldu.");

            TempData["Success"] = "Birim başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 UPDATE - GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Birim edit sayfası açıldı. Id: {Id}", id);

            var result = await _birimService.GetBirimByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Birim bulunamadı. Id: {Id}", id);

                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var updateVm = new UpdateBirimVm {
                Id = result.Data.Id,
                Ad = result.Data.Ad,
                Sira = result.Data.Sira,
                ParentId = result.Data.ParentId
            };

            var birimlerResult = await _birimService.GetBirimsAsync();
            var vm = new BirimEditIndexVm
            {
                Birim = updateVm,
                Birimler = birimlerResult.IsSuccess ? birimlerResult.Data! : new List<GetBirimVm>()
            };

            return View(vm);
        }

        // 🔹 UPDATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BirimEditIndexVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update Birim - ModelState geçersiz.");

                var birimlerResult = await _birimService.GetBirimsAsync();
                model.Birimler = birimlerResult.IsSuccess ? birimlerResult.Data! : new List<GetBirimVm>();
                return View(model);
            }

            var result = await _birimService.UpdateBirimAsync(model.Birim);

            if (!result.IsSuccess)
            {
                _logger.LogError("Birim güncellenemedi. Id: {Id}, Hata: {Error}", model.Birim.Id, result.Fail?.Detail);

                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");

                var birimlerResult = await _birimService.GetBirimsAsync();
                model.Birimler = birimlerResult.IsSuccess ? birimlerResult.Data! : new List<GetBirimVm>();
                return View(model);
            }

            _logger.LogInformation("Birim güncellendi. Id: {Id}", model.Birim.Id);

            TempData["Success"] = "Birim başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 DELETE - GET
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Birim delete sayfası açıldı. Id: {Id}", id);

            var result = await _birimService.GetBirimByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Birim bulunamadı. Id: {Id}", id);

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
            _logger.LogWarning("Birim silme isteği alındı. Id: {Id}", id);

            var result = await _birimService.DeleteBirimAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Birim silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);

                TempData["Error"] = result.Fail?.Detail
                                    ?? result.Fail?.Title
                                    ?? "Silme işlemi başarısız";

                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Birim başarıyla silindi. Id: {Id}", id);

            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 MOVE - AJAX (Drag & Drop ağaç sıralama)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoveBirim([FromBody] MoveBirimVm model)
        {
            if (model == null || model.Id <= 0)
            {
                return Json(new { success = false, message = "Geçersiz veri." });
            }

            _logger.LogInformation("Birim taşınıyor. Id: {Id}, ParentId: {ParentId}, Sira: {Sira}", model.Id, model.ParentId, model.Sira);

            // Önce mevcut birim verisini al
            var existingResult = await _birimService.GetBirimByIdAsync(model.Id);
            if (!existingResult.IsSuccess || existingResult.Data == null)
            {
                return Json(new { success = false, message = "Birim bulunamadı." });
            }

            var existing = existingResult.Data;

            // Taşınan birimi güncelle
            var updateVm = new UpdateBirimVm
            {
                Id = model.Id,
                ParentId = model.ParentId,
                Sira = model.Sira,
                Ad = existing.Ad
            };

            var result = await _birimService.UpdateBirimAsync(updateVm);

            if (!result.IsSuccess)
            {
                _logger.LogError("Birim taşınamadı. Id: {Id}, Hata: {Error}", model.Id, result.Fail?.Detail);
                return Json(new { success = false, message = result.Fail?.Detail ?? "Taşıma işlemi başarısız" });
            }

            // Tüm kardeşlerin Sira değerlerini yeni pozisyonlarına göre güncelle
            if (model.SiblingIds != null && model.SiblingIds.Count > 0)
            {
                for (int i = 0; i < model.SiblingIds.Count; i++)
                {
                    var siblingId = model.SiblingIds[i];
                    if (siblingId == model.Id) continue; // Zaten güncellendi

                    var siblingResult = await _birimService.GetBirimByIdAsync(siblingId);
                    if (siblingResult.IsSuccess && siblingResult.Data != null)
                    {
                        var siblingUpdateVm = new UpdateBirimVm
                        {
                            Id = siblingId,
                            ParentId = model.ParentId,
                            Sira = i,
                            Ad = siblingResult.Data.Ad
                        };
                        await _birimService.UpdateBirimAsync(siblingUpdateVm);
                    }
                }
            }

            _logger.LogInformation("Birim başarıyla taşındı. Id: {Id}", model.Id);
            return Json(new { success = true, message = "Birim başarıyla taşındı." });
        }

        // 🔹 jsTree JSON DATA - AJAX
        [HttpGet]
        public async Task<IActionResult> GetBirimTreeData()
        {
            var result = await _birimService.GetBirimsAsync();

            if (!result.IsSuccess || result.Data == null)
            {
                return Json(new List<object>());
            }

            var treeData = BuildJsTreeData(result.Data);
            return Json(treeData);
        }

        private List<object> BuildJsTreeData(List<GetBirimVm> birimler, int? parentId = null)
        {
            var items = birimler
                .Where(b => b.ParentId == parentId)
                .OrderBy(b => b.Sira)
                .Select(b => new Dictionary<string, object>
                {
                    { "id", b.Id },
                    { "text", b.Ad },
                    { "icon", "mdi mdi-office-building-marker" },
                    { "children", b.Children != null && b.Children.Any() ? BuildJsTreeData(b.Children.ToList(), b.Id) : new List<object>() },
                    { "data", new { sira = b.Sira, parentId = b.ParentId } },
                    { "a_attr", new { href = $"/Birim/Edit/{b.Id}" } }
                })
                .ToList<object>();

            return items;
        }

        private List<GetBirimVm> FlattenBirimlerList(List<GetBirimVm> birimler)
        {
            var result = new List<GetBirimVm>();
            foreach (var b in birimler)
            {
                result.Add(b);
                if (b.Children != null && b.Children.Any())
                {
                    result.AddRange(FlattenBirimlerList(b.Children));
                }
            }
            return result;
        }
    }
}