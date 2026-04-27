using Microservice.Admin.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class SiteController : Controller
    {
        private readonly ISiteService _siteService;
        public SiteController(ISiteService siteService)
        {
            _siteService = siteService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _siteService.GetSitesAsync();
            if (!result.IsSuccess)
            {
                // Handle error scenario, e.g., log the error, show an error message, etc.
                return View("Error");
            }

            return View(result.Data);
        }
    }
}
