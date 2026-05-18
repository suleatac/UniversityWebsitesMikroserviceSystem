using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index(string traceId)
        {
            ViewBag.TraceId = traceId;
            return View();
        }
    }
}
