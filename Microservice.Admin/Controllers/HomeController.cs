using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
        public IActionResult Index()
        {
            // Admin paneli açıldığında default olarak siteId=1 ve dilId=1 olsun
            if (!HttpContext.Session.GetInt32("CurrentSiteId").HasValue)
            {
                HttpContext.Session.SetInt32("CurrentSiteId", 1);
            }
            if (!HttpContext.Session.GetInt32("CurrentDilId").HasValue)
            {
                HttpContext.Session.SetInt32("CurrentDilId", 1);
            }

            return View();
        }
    }
}
