using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.SikcaSorulanSoru;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class SikcaSorulanSoruController : Controller
    {
        private readonly ISikcaSorulanSoruService _sikcaSorulanSoruService;
        private readonly ILogger<SikcaSorulanSoruController> _logger;

        public SikcaSorulanSoruController(
            ISikcaSorulanSoruService sikcaSorulanSoruService,
            ILogger<SikcaSorulanSoruController> logger)
        {
            _sikcaSorulanSoruService = sikcaSorulanSoruService;
            _logger = logger;
        }

        // 🔹 LIST - Tree view
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Sıkça Sorulan Soru listesi getiriliyor.");

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var result = await _sikcaSorulanSoruService.GetSikcaSorulanSorularAsync(currentSiteId, currentDilId);

            if (!result.IsSuccess)
            {
                _logger.LogError("Sıkça Sorulan Soru listesi alınamadı. Hata: {Error}", result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Sıkça Sorulan Soru listesi alınamadı.";
                return View(new List<GetSikcaSorulanSoruVm>());
            }

            return View(result.Data);
        }

        // 🔹 CREATE - GET
        [HttpGet]
        public async Task<IActionResult> Create(int? parentId)
        {
            _logger.LogInformation("Sıkça Sorulan Soru oluşturma sayfası açıldı. ParentId: {ParentId}", parentId);

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var sssResult = await _sikcaSorulanSoruService.GetSikcaSorulanSorularAsync(currentSiteId, currentDilId);
           

            var viewModel = new SikcaSorulanSoruCreateIndexVm
            {
                CreateSikcaSorulanSoru = new CreateSikcaSorulanSoruVm { ParentId = parentId, SiteId = currentSiteId, DilId = currentDilId },
     
                SikcaSorulanSorular = sssResult.IsSuccess ? sssResult.Data! : new List<GetSikcaSorulanSoruVm>()
            };

            return View(viewModel);
        }

        // 🔹 CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SikcaSorulanSoruCreateIndexVm model)
        {
            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            ModelState.Remove("CreateSikcaSorulanSoru.Sira");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create SSS - ModelState geçersiz.");
                var sssResult = await _sikcaSorulanSoruService.GetSikcaSorulanSorularAsync(currentSiteId, currentDilId);
                model.SikcaSorulanSorular = sssResult.IsSuccess ? sssResult.Data! : new List<GetSikcaSorulanSoruVm>();
                return View(model);
            }

            // Aynı parent altındaki maksimum Sira'yı bul ve +1 ata
            var allSss = await _sikcaSorulanSoruService.GetSikcaSorulanSorularAsync(currentSiteId, currentDilId);
            var flatSss = FlattenList(allSss.IsSuccess ? allSss.Data! : new List<GetSikcaSorulanSoruVm>());
            var siblings = flatSss.Where(m => m.ParentId == model.CreateSikcaSorulanSoru.ParentId).ToList();
            model.CreateSikcaSorulanSoru.Sira = siblings.Any() ? siblings.Max(m => m.Sira) + 1 : 1;

            model.CreateSikcaSorulanSoru.SiteId = currentSiteId;
            model.CreateSikcaSorulanSoru.DilId = currentDilId;

            var result = await _sikcaSorulanSoruService.CreateSikcaSorulanSoruAsync(model.CreateSikcaSorulanSoru);

            if (!result.IsSuccess)
            {
                _logger.LogError("SSS oluşturulamadı. Hata: {Error}", result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Sıkça Sorulan Soru oluşturulamadı.");   
                var sssResult = await _sikcaSorulanSoruService.GetSikcaSorulanSorularAsync(currentSiteId, currentDilId);
                model.SikcaSorulanSorular = sssResult.IsSuccess ? sssResult.Data! : new List<GetSikcaSorulanSoruVm>();
                return View(model);
            }

            _logger.LogInformation("SSS oluşturuldu. Soru: {Soru}", model.CreateSikcaSorulanSoru.Soru);
            TempData["Success"] = "Sıkça Sorulan Soru başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 UPDATE - GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("SSS düzenleme sayfası açıldı. Id: {Id}", id);

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var result = await _sikcaSorulanSoruService.GetSikcaSorulanSoruByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("SSS bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var sssResult = await _sikcaSorulanSoruService.GetSikcaSorulanSorularAsync(currentSiteId, currentDilId);

            var vm = new SikcaSorulanSoruEditIndexVm
            {
                SikcaSorulanSoru = result.Data,
                SikcaSorulanSorular = sssResult.IsSuccess ? sssResult.Data! : new List<GetSikcaSorulanSoruVm>()
            };

            return View(vm);
        }

        // 🔹 UPDATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SikcaSorulanSoruEditIndexVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update SSS - ModelState geçersiz.");
                var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
                var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;
                var sssResult = await _sikcaSorulanSoruService.GetSikcaSorulanSorularAsync(currentSiteId, currentDilId);
                model.SikcaSorulanSorular = sssResult.IsSuccess ? sssResult.Data! : new List<GetSikcaSorulanSoruVm>();
                return View(model);
            }

            var result = await _sikcaSorulanSoruService.UpdateSikcaSorulanSoruAsync(model.SikcaSorulanSoru);

            if (!result.IsSuccess)
            {
                _logger.LogError("SSS güncellenemedi. Id: {Id}, Hata: {Error}", model.SikcaSorulanSoru.Id, result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");
                var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
                var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;
                var sssResult = await _sikcaSorulanSoruService.GetSikcaSorulanSorularAsync(currentSiteId, currentDilId);
                model.SikcaSorulanSorular = sssResult.IsSuccess ? sssResult.Data! : new List<GetSikcaSorulanSoruVm>();
                return View(model);
            }

            _logger.LogInformation("SSS güncellendi. Id: {Id}", model.SikcaSorulanSoru.Id);
            TempData["Success"] = "Sıkça Sorulan Soru başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 DELETE - GET
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("SSS delete sayfası açıldı. Id: {Id}", id);

            var result = await _sikcaSorulanSoruService.GetSikcaSorulanSoruByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("SSS bulunamadı. Id: {Id}", id);
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
            _logger.LogWarning("SSS silme isteği alındı. Id: {Id}", id);

            var result = await _sikcaSorulanSoruService.DeleteSikcaSorulanSoruAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("SSS silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Silme işlemi başarısız";
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("SSS başarıyla silindi. Id: {Id}", id);
            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 MOVE - AJAX (Drag & Drop ağaç sıralama)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoveSikcaSorulanSoru([FromBody] MoveSikcaSorulanSoruVm model)
        {
            if (model == null || model.Id <= 0)
            {
                return Json(new { success = false, message = "Geçersiz veri." });
            }

            _logger.LogInformation("SSS taşınıyor. Id: {Id}, ParentId: {ParentId}, Sira: {Sira}", model.Id, model.ParentId, model.Sira);

            // Önce mevcut veriyi al
            var existingResult = await _sikcaSorulanSoruService.GetSikcaSorulanSoruByIdAsync(model.Id);
            if (!existingResult.IsSuccess || existingResult.Data == null)
            {
                return Json(new { success = false, message = "SSS bulunamadı." });
            }

            var existing = existingResult.Data;

            // Taşınan kaydı güncelle
            var updateVm = new SikcaSorulanSoruDetailVm
            {
                Id = model.Id,
                ParentId = model.ParentId,
                Sira = model.Sira,
                SiteId = existing.SiteId,
                DilId = existing.DilId,
                Soru = existing.Soru,
                Cevap = existing.Cevap,
                SeoUrl = existing.SeoUrl
            };

            var result = await _sikcaSorulanSoruService.UpdateSikcaSorulanSoruAsync(updateVm);

            if (!result.IsSuccess)
            {
                _logger.LogError("SSS taşınamadı. Id: {Id}, Hata: {Error}", model.Id, result.Fail?.Detail);
                return Json(new { success = false, message = result.Fail?.Detail ?? "Taşıma işlemi başarısız" });
            }

            // Tüm kardeşlerin Sira değerlerini yeni pozisyonlarına göre güncelle
            if (model.SiblingIds != null && model.SiblingIds.Count > 0)
            {
                for (int i = 0; i < model.SiblingIds.Count; i++)
                {
                    var siblingId = model.SiblingIds[i];
                    if (siblingId == model.Id) continue; // Zaten güncellendi

                    var siblingResult = await _sikcaSorulanSoruService.GetSikcaSorulanSoruByIdAsync(siblingId);
                    if (siblingResult.IsSuccess && siblingResult.Data != null)
                    {
                        var siblingUpdateVm = new SikcaSorulanSoruDetailVm
                        {
                            Id = siblingId,
                            ParentId = model.ParentId,
                            Sira = i,
                            SiteId = siblingResult.Data.SiteId,
                            DilId = siblingResult.Data.DilId,
                            Soru = siblingResult.Data.Soru,
                            Cevap = siblingResult.Data.Cevap,
                            SeoUrl = siblingResult.Data.SeoUrl
                        };
                        await _sikcaSorulanSoruService.UpdateSikcaSorulanSoruAsync(siblingUpdateVm);
                    }
                }
            }

            _logger.LogInformation("SSS başarıyla taşındı. Id: {Id}", model.Id);
            return Json(new { success = true, message = "SSS başarıyla taşındı." });
        }

        // 🔹 jsTree JSON DATA - AJAX
        [HttpGet]
        public async Task<IActionResult> GetTreeData()
        {
            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var result = await _sikcaSorulanSoruService.GetSikcaSorulanSorularAsync(currentSiteId, currentDilId);

            if (!result.IsSuccess || result.Data == null)
            {
                return Json(new List<object>());
            }

            var treeData = BuildJsTreeData(result.Data);
            return Json(treeData);
        }

        private List<object> BuildJsTreeData(List<GetSikcaSorulanSoruVm> sssList, int? parentId = null)
        {
            var items = sssList
                .Where(m => m.ParentId == parentId)
                .OrderBy(m => m.Sira)
                .Select(m => new Dictionary<string, object>
                {
                    { "id", m.Id },
                    { "text", m.Soru },
                    { "icon", "mdi mdi-help-circle" },
                    { "children", m.Children != null && m.Children.Any() ? BuildJsTreeData(m.Children.ToList(), m.Id) : new List<object>() },
                    { "data", new { sira = m.Sira, parentId = m.ParentId } },
                    { "a_attr", new { href = $"/SikcaSorulanSoru/Edit/{m.Id}" } }
                })
                .ToList<object>();

            return items;
        }

        private List<GetSikcaSorulanSoruVm> FlattenList(List<GetSikcaSorulanSoruVm> sssList)
        {
            var result = new List<GetSikcaSorulanSoruVm>();
            foreach (var m in sssList)
            {
                result.Add(m);
                if (m.Children != null && m.Children.Any())
                {
                    result.AddRange(FlattenList(m.Children));
                }
            }
            return result;
        }
    }
}
