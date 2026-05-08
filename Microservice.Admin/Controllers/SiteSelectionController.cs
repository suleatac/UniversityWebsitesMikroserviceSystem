using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class SiteSelectionController : Controller
    {
        /// <summary>
        /// Session'a seçilen site Id'sini kaydeder.
        /// Default: 1 (Türkçe site)
        /// </summary>
        [HttpPost]
        public IActionResult SetSite(int siteId)
        {
            if (siteId <= 0)
            {
                return Json(new { success = false, message = "Geçersiz site Id." });
            }

            HttpContext.Session.SetInt32("CurrentSiteId", siteId);
            return Json(new { success = true, message = "Site seçimi güncellendi.", siteId = siteId });
        }

        /// <summary>
        /// Session'a seçilen dil Id'sini kaydeder.
        /// Default: 1 (Türkçe), İngilizce: 2
        /// </summary>
        [HttpPost]
        public IActionResult SetDil(int dilId)
        {
            if (dilId <= 0)
            {
                return Json(new { success = false, message = "Geçersiz dil Id." });
            }

            HttpContext.Session.SetInt32("CurrentDilId", dilId);
            return Json(new { success = true, message = "Dil seçimi güncellendi.", dilId = dilId });
        }

        /// <summary>
        /// Mevcut site ve dil seçimini JSON olarak döner.
        /// </summary>
        [HttpGet]
        public IActionResult GetCurrentSelection()
        {
            var siteId = HttpContext.Session.GetInt32("CurrentSiteId") ?? 1;
            var dilId = HttpContext.Session.GetInt32("CurrentDilId") ?? 1;

            return Json(new { siteId, dilId });
        }
    }
}