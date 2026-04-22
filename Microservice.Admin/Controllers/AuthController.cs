using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UserAdd()
        {
            return View();
        }
    }
}
