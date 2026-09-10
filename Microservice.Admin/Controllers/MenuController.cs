using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.Dil;
using Microservice.Admin.ViewModels.Hedef;
using Microservice.Admin.ViewModels.Menu;
using Microservice.Admin.ViewModels.PageType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly IDilService _dilService;
        private readonly IHedefService _hedefService;
        private readonly ILogger<MenuController> _logger;

        public MenuController(
            IMenuService menuService,
            IDilService dilService,
            IHedefService hedefService,
            ILogger<MenuController> logger)
        {
            _menuService = menuService;
            _dilService = dilService;
            _hedefService = hedefService;
            _logger = logger;
        }

        // 🔹 LIST
        public async Task<IActionResult> Index(int? location)
        {
            _logger.LogInformation("Menu listesi getiriliyor. Location: {Location}", location);

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var result = await _menuService.GetMenusAsync(currentSiteId, currentDilId, location);

            if (!result.IsSuccess)
            {
                _logger.LogError("Menu listesi alınamadı. Hata: {Error}", result.Fail?.Detail);

                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Menu listesi alınamadı.";
                return View("Error");
            }

            _logger.LogInformation("Menu listesi başarıyla getirildi. Count: {Count}", result.Data!.Count);

            ViewBag.Location = location;
            return View(result.Data);
        }

        // 🔹 CREATE - GET
        [HttpGet]
        public async Task<IActionResult> Create(int? parentId, int? location)
        {
            _logger.LogInformation("Menu oluşturma sayfası açıldı. ParentId: {ParentId}, Location: {Location}", parentId, location);

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            // Parent secildiyse konumu parent'tan miras al (footer sutununun altindaki link footer olmali)
            var inheritedLocation = location;
            if (parentId.HasValue && (inheritedLocation is null || inheritedLocation == MenuLocationOption.Header))
            {
                var parentResult = await _menuService.GetMenuByIdAsync(parentId.Value);
                if (parentResult.IsSuccess && parentResult.Data != null)
                {
                    inheritedLocation = parentResult.Data.Location;
                }
            }

            var menulerResult = await _menuService.GetMenusAsync(currentSiteId, currentDilId);
            var dillerResult = await _dilService.GetDilsAsync();
            var hedeflerResult = await _hedefService.GetHedefsAsync();

            var vm = new MenuCreateIndexVm
            {
                CreateMenu = new MenuDetailVm
                {
                    ParentId = parentId,
                    SiteId = currentSiteId,
                    DilId = currentDilId,
                    Location = inheritedLocation ?? MenuLocationOption.Header
                },
                Menuler = menulerResult.IsSuccess ? menulerResult.Data! : new List<GetMenuVm>(),
                Diller = dillerResult.IsSuccess ? dillerResult.Data! : new List<GetDilVm>(),
                Hedefler = hedeflerResult.IsSuccess ? hedeflerResult.Data! : new List<GetHedefVm>()
            };

            return View(vm);
        }

        // 🔹 CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuCreateIndexVm model)
        {
            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            // Sira alanı formdan gelmiyor, otomatik hesaplanacak
            ModelState.Remove("CreateMenu.Sira");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create Menu - ModelState geçersiz.");

                var menulerResult = await _menuService.GetMenusAsync(currentSiteId, currentDilId);
                var dillerResult = await _dilService.GetDilsAsync();
                var hedeflerResult = await _hedefService.GetHedefsAsync();

                model.Menuler = menulerResult.IsSuccess ? menulerResult.Data! : new List<GetMenuVm>();
                model.Diller = dillerResult.IsSuccess ? dillerResult.Data! : new List<GetDilVm>();
                model.Hedefler = hedeflerResult.IsSuccess ? hedeflerResult.Data! : new List<GetHedefVm>();
                return View(model);
            }

            // Aynı parent altındaki maksimum Sira'yı bul ve +1 ata
            var allMenuler = await _menuService.GetMenusAsync(currentSiteId, currentDilId);
            var flatMenuler = FlattenMenulerList(allMenuler.IsSuccess ? allMenuler.Data! : new List<GetMenuVm>());
            var siblings = flatMenuler.Where(m => m.ParentId == model.CreateMenu.ParentId).ToList();
            model.CreateMenu.Sira = siblings.Any() ? siblings.Max(m => m.Sira) + 1 : 1;

            // SiteId ve DilId'yi session'dan set et
            model.CreateMenu.SiteId = currentSiteId;
            model.CreateMenu.DilId = currentDilId;

            var result = await _menuService.CreateMenuAsync(model.CreateMenu);

            if (!result.IsSuccess)
            {
                _logger.LogError("Menu oluşturulamadı. Hata: {Error}", result.Fail?.Detail);

                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Menu oluşturulamadı.");

                var menulerResult = await _menuService.GetMenusAsync(currentSiteId, currentDilId);
                var dillerResult = await _dilService.GetDilsAsync();
                var hedeflerResult = await _hedefService.GetHedefsAsync();

                model.Menuler = menulerResult.IsSuccess ? menulerResult.Data! : new List<GetMenuVm>();
                model.Diller = dillerResult.IsSuccess ? dillerResult.Data! : new List<GetDilVm>();
                model.Hedefler = hedeflerResult.IsSuccess ? hedeflerResult.Data! : new List<GetHedefVm>();
                return View(model);
            }

            _logger.LogInformation("Menu oluşturuldu.");

            TempData["Success"] = "Menu başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 UPDATE - GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Menu edit sayfası açıldı. Id: {Id}", id);

            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var result = await _menuService.GetMenuByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Menu bulunamadı. Id: {Id}", id);

                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var updateVm = new MenuDetailVm
            {
                Id = result.Data.Id,
                PageTypeId = result.Data.PageTypeId,
                SiteId = result.Data.SiteId,
                DilId = result.Data.DilId,
                HedefId = result.Data.HedefId,
                Baslik = result.Data.Baslik,
                Link = result.Data.Link,
                IcerikMetni = result.Data.IcerikMetni,
                Sira = result.Data.Sira,
                MegaMenu = result.Data.MegaMenu,
                Location = result.Data.Location,
                IsVisible = result.Data.IsVisible,
                ParentId = result.Data.ParentId
            };

            var menulerResult = await _menuService.GetMenusAsync(currentSiteId, currentDilId);
            var dillerResult = await _dilService.GetDilsAsync();
            var hedeflerResult = await _hedefService.GetHedefsAsync();

            var vm = new MenuEditIndexVm
            {
                Menu = updateVm,
                Menuler = menulerResult.IsSuccess ? menulerResult.Data! : new List<GetMenuVm>(),
                Diller = dillerResult.IsSuccess ? dillerResult.Data! : new List<GetDilVm>(),
                Hedefler = hedeflerResult.IsSuccess ? hedeflerResult.Data! : new List<GetHedefVm>()
            };

            return View(vm);
        }

        // 🔹 UPDATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MenuEditIndexVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update Menu - ModelState geçersiz.");

                var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
                var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

                var menulerResult = await _menuService.GetMenusAsync(currentSiteId, currentDilId);
                var dillerResult = await _dilService.GetDilsAsync();
                var hedeflerResult = await _hedefService.GetHedefsAsync();

                model.Menuler = menulerResult.IsSuccess ? menulerResult.Data! : new List<GetMenuVm>();
                model.Diller = dillerResult.IsSuccess ? dillerResult.Data! : new List<GetDilVm>();
                model.Hedefler = hedeflerResult.IsSuccess ? hedeflerResult.Data! : new List<GetHedefVm>();
                return View(model);
            }

            var result = await _menuService.UpdateMenuAsync(model.Menu);

            if (!result.IsSuccess)
            {
                _logger.LogError("Menu güncellenemedi. Id: {Id}, Hata: {Error}", model.Menu.Id, result.Fail?.Detail);

                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Güncelleme başarısız");

                var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
                var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

                var menulerResult = await _menuService.GetMenusAsync(currentSiteId, currentDilId);
                var dillerResult = await _dilService.GetDilsAsync();
                var hedeflerResult = await _hedefService.GetHedefsAsync();

                model.Menuler = menulerResult.IsSuccess ? menulerResult.Data! : new List<GetMenuVm>();
                model.Diller = dillerResult.IsSuccess ? dillerResult.Data! : new List<GetDilVm>();
                model.Hedefler = hedeflerResult.IsSuccess ? hedeflerResult.Data! : new List<GetHedefVm>();
                return View(model);
            }

            _logger.LogInformation("Menu güncellendi. Id: {Id}", model.Menu.Id);

            TempData["Success"] = "Menu başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 DELETE - GET
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Menu delete sayfası açıldı. Id: {Id}", id);

            var result = await _menuService.GetMenuByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Menu bulunamadı. Id: {Id}", id);

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
            _logger.LogWarning("Menu silme isteği alındı. Id: {Id}", id);

            var result = await _menuService.DeleteMenuAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Menu silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);

                TempData["Error"] = result.Fail?.Detail
                                    ?? result.Fail?.Title
                                    ?? "Silme işlemi başarısız";

                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Menu başarıyla silindi. Id: {Id}", id);

            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 MOVE - AJAX (Drag & Drop ağaç sıralama)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoveMenu([FromBody] MoveMenuVm model)
        {
            if (model == null || model.Id <= 0)
            {
                return Json(new { success = false, message = "Geçersiz veri." });
            }

            _logger.LogInformation("Menu taşınıyor. Id: {Id}, ParentId: {ParentId}, Sira: {Sira}", model.Id, model.ParentId, model.Sira);

            // Önce mevcut menu verisini al
            var existingResult = await _menuService.GetMenuByIdAsync(model.Id);
            if (!existingResult.IsSuccess || existingResult.Data == null)
            {
                return Json(new { success = false, message = "Menu bulunamadı." });
            }

            var existing = existingResult.Data;

            // Taşınan menuyu güncelle
            var updateVm = new MenuDetailVm
            {
                Id = model.Id,
                ParentId = model.ParentId,
                Sira = model.Sira,
                Baslik = existing.Baslik,
                Link = existing.Link,
                IcerikMetni = existing.IcerikMetni,
                MegaMenu = existing.MegaMenu,
                Location = existing.Location,
                IsVisible = existing.IsVisible,
                SiteId = existing.SiteId,
                DilId = existing.DilId,
                HedefId = existing.HedefId
            };

            var result = await _menuService.UpdateMenuAsync(updateVm);

            if (!result.IsSuccess)
            {
                _logger.LogError("Menu taşınamadı. Id: {Id}, Hata: {Error}", model.Id, result.Fail?.Detail);
                return Json(new { success = false, message = result.Fail?.Detail ?? "Taşıma işlemi başarısız" });
            }

            // Tüm kardeşlerin Sira değerlerini yeni pozisyonlarına göre güncelle
            if (model.SiblingIds != null && model.SiblingIds.Count > 0)
            {
                for (int i = 0; i < model.SiblingIds.Count; i++)
                {
                    var siblingId = model.SiblingIds[i];
                    if (siblingId == model.Id) continue; // Zaten güncellendi

                    var siblingResult = await _menuService.GetMenuByIdAsync(siblingId);
                    if (siblingResult.IsSuccess && siblingResult.Data != null)
                    {
                        var siblingUpdateVm = new MenuDetailVm
                        {
                            Id = siblingId,
                            ParentId = model.ParentId,
                            Sira = i,
                            Baslik = siblingResult.Data.Baslik,
                            Link = siblingResult.Data.Link,
                            IcerikMetni = siblingResult.Data.IcerikMetni,
                            MegaMenu = siblingResult.Data.MegaMenu,
                            Location = siblingResult.Data.Location,
                            IsVisible = siblingResult.Data.IsVisible,
                            SiteId = siblingResult.Data.SiteId,
                            DilId = siblingResult.Data.DilId,
                            HedefId = siblingResult.Data.HedefId
                        };
                        await _menuService.UpdateMenuAsync(siblingUpdateVm);
                    }
                }
            }

            _logger.LogInformation("Menu başarıyla taşındı. Id: {Id}", model.Id);
            return Json(new { success = true, message = "Menu başarıyla taşındı." });
        }

        // 🔹 jsTree JSON DATA - AJAX
        [HttpGet]
        public async Task<IActionResult> GetMenuTreeData(int? location)
        {
            var currentSiteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var currentDilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            var result = await _menuService.GetMenusAsync(currentSiteId, currentDilId, location);

            if (!result.IsSuccess || result.Data == null)
            {
                return Json(new List<object>());
            }

            var treeData = BuildJsTreeData(result.Data);
            return Json(treeData);
        }

        private List<object> BuildJsTreeData(List<GetMenuVm> menuler, int? parentId = null)
        {
            var items = menuler
                .Where(m => m.ParentId == parentId)
                .OrderBy(m => m.Sira)
                .Select(m => new Dictionary<string, object>
                {
                    { "id", m.Id },
                    { "text", m.Baslik },
                    { "icon", "mdi mdi-menu" },
                    { "children", m.Children != null && m.Children.Any() ? BuildJsTreeData(m.Children.ToList(), m.Id) : new List<object>() },
                    { "data", new { sira = m.Sira, parentId = m.ParentId } },
                    { "a_attr", new { href = $"/Menu/Edit/{m.Id}" } }
                })
                .ToList<object>();

            return items;
        }

        private List<GetMenuVm> FlattenMenulerList(List<GetMenuVm> menuler)
        {
            var result = new List<GetMenuVm>();
            foreach (var m in menuler)
            {
                result.Add(m);
                if (m.Children != null && m.Children.Any())
                {
                    result.AddRange(FlattenMenulerList(m.Children));
                }
            }
            return result;
        }
    }
}