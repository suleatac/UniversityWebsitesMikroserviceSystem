using Microservice.Web.Models;
using Microservice.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Microservice.Web.Controllers
{
  

    public class HomeController : Controller
    {
        private readonly IHaberService _haberService;

        public HomeController(IHaberService haberService)
        {
            _haberService = haberService;
        }

        public async Task<IActionResult> Index()
        {
          
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var result = await _haberService.GetHabersAsync(1, 1);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
