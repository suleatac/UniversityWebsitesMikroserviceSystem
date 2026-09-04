using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.Dil;
using Microservice.Admin.ViewModels.PageType;
using Microservice.Admin.ViewModels.Template;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class PageTypeController(
        IPageTypeService pageTypeService,
        IDilService dilService,
        ITemplateService templateService,
        ILogger<PageTypeController> logger) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await pageTypeService.GetPageTypesAsync();
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "PageType listesi alınamadı.";
                return View(new List<GetPageTypeVm>());
            }

            return View(result.Data ?? []);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulatePageTypeViewBagsAsync();
            return View(new CreatePageTypeVm ());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePageTypeVm model)
        {
            if (!ModelState.IsValid)
            {
                await PopulatePageTypeViewBagsAsync();
                return View(model);
            }

            var result = await pageTypeService.CreatePageTypeAsync(model);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "PageType oluşturulamadı.");
                await PopulatePageTypeViewBagsAsync();
                return View(model);
            }

            TempData["Success"] = "PageType başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await pageTypeService.GetPageTypeByIdAsync(id);
            if (!result.IsSuccess || result.Data is null)
            {
                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            await PopulatePageTypeViewBagsAsync();
            return View(new UpdatePageTypeVm
            {
                Id = result.Data.Id,
                PageTypeKind = result.Data.PageTypeKind,
                DilId = result.Data.DilId,
                Name = result.Data.Name,
                Slug = result.Data.Slug,
                TemplateId = result.Data.TemplateId,
                
                ViewName = result.Data.ViewName,
                IsHomePage = result.Data.IsHomePage,
                IsActive = result.Data.IsActive
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdatePageTypeVm model)
        {
            if (!ModelState.IsValid)
            {
                await PopulatePageTypeViewBagsAsync();
                return View(model);
            }

            var result = await pageTypeService.UpdatePageTypeAsync(model);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "PageType güncellenemedi.");
                await PopulatePageTypeViewBagsAsync();
                return View(model);
            }

            TempData["Success"] = "PageType başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await pageTypeService.GetPageTypeByIdAsync(id);
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
            var result = await pageTypeService.DeletePageTypeAsync(id);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Silme işlemi başarısız.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "PageType başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulatePageTypeViewBagsAsync()
        {
            ViewBag.PageTypeler = Enum.GetValues<PageTypeKind>()
                .Select(kind => new { Id = (int)kind, Ad = kind.ToString() })
                .ToList();

            var dillerResult = await dilService.GetDilsAsync();
            ViewBag.Diller = dillerResult.IsSuccess ? dillerResult.Data! : new List<GetDilVm>();

            var templatesResult = await templateService.GetTemplatesAsync();
            ViewBag.Templates = templatesResult.IsSuccess ? templatesResult.Data! : new List<GetTemplateVm>();
        }
    }
}