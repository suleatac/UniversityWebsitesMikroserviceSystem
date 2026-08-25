using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.PageType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class PageTypeController(IPageTypeService pageTypeService, ILogger<PageTypeController> logger) : Controller
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
        public IActionResult Create() => View(new CreatePageTypeVm { SiteId = 1, DilId = 1, TemplateId = 1 });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePageTypeVm model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await pageTypeService.CreatePageTypeAsync(model);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "PageType oluşturulamadı.");
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

            return View(new UpdatePageTypeVm
            {
                Id = result.Data.Id,
                PageTypeId = result.Data.PageTypeId,
                SiteId = result.Data.SiteId,
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
                return View(model);

            var result = await pageTypeService.UpdatePageTypeAsync(model);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "PageType güncellenemedi.");
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
    }
}