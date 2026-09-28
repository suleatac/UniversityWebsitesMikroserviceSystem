using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.PageBuilder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    /// <summary>
    /// Ana sayfa dinamik bolum (PageSection) ve container (PageBlock) yonetimi.
    /// Site secimi session'dan alinir; bolumler dil bazinda yonetilir.
    /// </summary>
    [Authorize]
    public class PageBuilderController : Controller
    {
        private readonly IPageSectionService _pageSectionService;
        private readonly ILogger<PageBuilderController> _logger;

        public PageBuilderController(
            IPageSectionService pageSectionService,
            ILogger<PageBuilderController> logger)
        {
            _pageSectionService = pageSectionService;
            _logger = logger;
        }

        private int CurrentSiteId => HttpContext.Session.GetInt32("CurrentSiteId") ?? 0;
        private int CurrentDilId => HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentSiteId = CurrentSiteId;
            if (currentSiteId <= 0)
            {
                TempData["Error"] = "Lütfen önce site seçiniz.";
                return RedirectToAction("SelectSite", "SiteSelection");
            }

            var result = await _pageSectionService.GetPageSectionsAsync(currentSiteId, CurrentDilId);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Bölüm listesi alınamadı. SiteId: {SiteId}, Hata: {Error}", currentSiteId, result.Fail?.Detail);
                return View(new List<GetPageSectionVm>());
            }

            return View(result.Data ?? []);
        }

        // ============================================================
        // SECTION CRUD
        // ============================================================

        [HttpGet]
        public IActionResult Create()
        {
            var model = new PageSectionDetailVm {
                SiteId = CurrentSiteId,
                DilId = CurrentDilId,
                Yayinda = true
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PageSectionDetailVm model)
        {
            // SiteId/DilId her zaman session'dan gelmelidir (form alanlari manipüle edilebilir).
            model.SiteId = CurrentSiteId;
            model.DilId = CurrentDilId;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _pageSectionService.CreatePageSectionAsync(model);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Bölüm oluşturulamadı.");
                return View(model);
            }

            TempData["Success"] = "Bölüm başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _pageSectionService.GetPageSectionByIdAsync(id);

            if (!result.IsSuccess || result.Data is null)
            {
                TempData["Error"] = "Bölüm bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var section = result.Data;
            var model = new PageSectionDetailVm {
                Id = section.Id,
                SiteId = section.SiteId,
                DilId = section.DilId,
                Baslik = section.Baslik,
                BackgroundColor = section.BackgroundColor,
                BackgroundImageUrl = section.BackgroundImageUrl,
                Sira = section.Sira,
                Yayinda = section.Yayinda
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PageSectionDetailVm model)
        {
            model.SiteId = CurrentSiteId;
            model.DilId = CurrentDilId;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _pageSectionService.UpdatePageSectionAsync(model);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Bölüm güncellenemedi.");
                return View(model);
            }

            TempData["Success"] = "Bölüm başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _pageSectionService.GetPageSectionByIdAsync(id);

            if (!result.IsSuccess || result.Data is null)
            {
                TempData["Error"] = "Silinecek kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _pageSectionService.DeletePageSectionAsync(id);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Fail?.Detail ?? "Silme işlemi başarısız.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Bölüm ve tüm containerları silindi.";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Index sayfasindaki Surukle-Birak siralama icin AJAX ucu.
        /// Token, gorseldeki gizli antiforgery formundan header olarak gonderilir (Banner örnegi).
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReorderSections([FromBody] List<ReorderPageSectionItemVm> items)
        {
            if (items == null || items.Count == 0)
            {
                return Json(new { success = false, message = "Geçersiz veri." });
            }

            var result = await _pageSectionService.ReorderPageSectionsAsync(items);

            if (!result.IsSuccess)
            {
                return Json(new { success = false, message = result.Fail?.Detail ?? "Sıralama güncellenemedi." });
            }

            return Json(new { success = true, message = "Sıralama başarıyla güncellendi." });
        }

        // ============================================================
        // BLOCK (CONTAINER) YÖNETIMI
        // ============================================================

        /// <summary>
        /// Bir bolumun container (block) agaci sayfası.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Blocks(int sectionId)
        {
            var result = await _pageSectionService.GetPageSectionByIdAsync(sectionId);

            if (!result.IsSuccess || result.Data is null)
            {
                TempData["Error"] = "Bölüm bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Section = result.Data;

            // Ust block secimi icin duz liste (indentli etiketlerle).
            var parentOptions = new List<PageBlockSelectItemVm>();
            FlattenBlocks(result.Data.Blocks, 0, parentOptions);
            ViewBag.ParentOptions = parentOptions;

            return View(result.Data);
        }

        private static void FlattenBlocks(List<GetPageBlockVm> blocks, int depth, List<PageBlockSelectItemVm> options)
        {
            foreach (var block in blocks)
            {
                var label = $"{new string('─', depth * 2)} [{block.ContentType}] {(string.IsNullOrWhiteSpace(block.Content) ? $"#{block.Id}" : Truncate(block.Content, 40))}";
                options.Add(new PageBlockSelectItemVm { Id = block.Id, Label = label });

                FlattenBlocks(block.Children, depth + 1, options);
            }
        }

        private static string Truncate(string text, int max)
        {
            var plain = System.Text.RegularExpressions.Regex.Replace(text, "<.*?>", string.Empty).Trim();
            return plain.Length <= max ? plain : plain[..max] + "…";
        }

        [HttpGet]
        public async Task<IActionResult> CreateBlock(int sectionId, int? parentId = null, int? editBlockId = null)
        {
            var sectionResult = await _pageSectionService.GetPageSectionByIdAsync(sectionId);
            if (!sectionResult.IsSuccess || sectionResult.Data is null)
            {
                TempData["Error"] = "Bölüm bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            // editBlockId verilmis ise bolum agacindaki block verisiyle formu doldur (API'den gelir).
            PageBlockFormVm model;
            var editBlock = editBlockId is null
                ? null
                : FindBlock(sectionResult.Data.Blocks, editBlockId.Value);

            if (editBlock is not null)
            {
                model = new PageBlockFormVm {
                    Id = editBlock.Id,
                    PageSectionId = editBlock.PageSectionId,
                    ParentId = editBlock.ParentId,
                    ContentType = editBlock.ContentType,
                    Content = editBlock.Content,
                    VideoUrl = editBlock.VideoUrl,
                    VideoType = editBlock.VideoType,
                    BackgroundImageUrl = editBlock.BackgroundImageUrl,
                    BackgroundColor = editBlock.BackgroundColor,
                    ColumnSize = editBlock.ColumnSize,
                    RowNumber = editBlock.RowNumber,
                    Animation = editBlock.Animation,
                    Medias = editBlock.Medias
                        .OrderBy(m => m.Sira)
                        .Select(m => new PageBlockMediaInputVm {
                            ResimUrl = m.ResimUrl,
                            VideoUrl = m.VideoUrl,
                            Sira = m.Sira
                        })
                        .ToList()
                };
            }
            else
            {
                model = new PageBlockFormVm {
                    PageSectionId = sectionId,
                    ParentId = parentId,
                    ColumnSize = parentId is null ? 12 : 6
                };
            }

            ViewBag.Section = sectionResult.Data;
            LoadParentOptions(sectionResult.Data, model, model.ParentId);

            return View(model);
        }

        /// <summary>Bolum agacinda (children recursive) id ile block arar.</summary>
        private static GetPageBlockVm? FindBlock(List<GetPageBlockVm> blocks, int id)
        {
            foreach (var block in blocks)
            {
                if (block.Id == id) return block;

                var found = FindBlock(block.Children, id);
                if (found is not null) return found;
            }

            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBlock(PageBlockFormVm model)
        {
            if (!ModelState.IsValid)
            {
                await ReloadBlockPageContextAsync(model);
                return View(model);
            }

            var result = await _pageSectionService.CreatePageBlockAsync(model.PageSectionId, model);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Container oluşturulamadı.");
                await ReloadBlockPageContextAsync(model);
                return View(model);
            }

            TempData["Success"] = "Container başarıyla eklendi.";
            return RedirectToAction(nameof(Blocks), new { sectionId = model.PageSectionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBlock(PageBlockFormVm model)
        {
            if (!ModelState.IsValid)
            {
                await ReloadBlockPageContextAsync(model);
                return View(nameof(CreateBlock), model);
            }

            var result = await _pageSectionService.UpdatePageBlockAsync(model);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Container güncellenemedi.");
                await ReloadBlockPageContextAsync(model);
                return View(nameof(CreateBlock), model);
            }

            TempData["Success"] = "Container başarıyla güncellendi.";
            return RedirectToAction(nameof(Blocks), new { sectionId = model.PageSectionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBlock(int blockId, int sectionId)
        {
            var result = await _pageSectionService.DeletePageBlockAsync(blockId);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Fail?.Detail ?? "Container silinemedi.";
            }
            else
            {
                TempData["Success"] = "Container silindi.";
            }

            return RedirectToAction(nameof(Blocks), new { sectionId });
        }

        private async Task ReloadBlockPageContextAsync(PageBlockFormVm model)
        {
            var sectionResult = await _pageSectionService.GetPageSectionByIdAsync(model.PageSectionId);
            if (sectionResult.IsSuccess && sectionResult.Data is not null)
            {
                ViewBag.Section = sectionResult.Data;
                LoadParentOptions(sectionResult.Data, model, model.ParentId);
            }
        }

        private void LoadParentOptions(GetPageSectionVm section, PageBlockFormVm model, int? selectedParentId)
        {
            var parentOptions = new List<PageBlockSelectItemVm>();
            FlattenBlocks(section.Blocks, 0, parentOptions);

            // Kendi id'si secilemez (parent dongusu olmasin).
            if (model.Id > 0)
            {
                parentOptions.RemoveAll(o => o.Id == model.Id);
            }

            ViewBag.ParentOptions = parentOptions;
            ViewBag.SelectedParentId = selectedParentId;
        }
    }
}
