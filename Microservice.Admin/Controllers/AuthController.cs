using Microservice.Admin.Attributes;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.SignIn;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService authService;
        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [SkipAudit]
        public IActionResult SignIn()
        {
            // Yetkisi olmayan kullanıcı logout edildikten sonra gelen uyarı mesajını göster
            if (TempData["Warning"] != null)
            {
                ModelState.AddModelError(string.Empty, TempData["Warning"]!.ToString()!);
            }
            return View();
        }
        [SkipAudit]
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInVm signInViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(signInViewModel);
            }
          
            var result = await authService.AuthenticateAsync(signInViewModel);

            if (result.IsSuccess)
            {
                // Redirect to a success page or login page
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Fail!.Title!);
                if (string.IsNullOrEmpty(result.Fail.Detail) == false)
                {
                    ModelState.AddModelError(string.Empty, result.Fail.Detail);
                }
                return View(signInViewModel);
            }
        }

        [HttpPost]
        [SkipAudit]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Clear();

            TempData.Clear();

            return RedirectToAction("SignIn");
        }
        [SkipAudit]
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
