using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.SignIn;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Microservice.Admin.Services
{
    public class AuthService(IHttpContextAccessor httpContextAccessor, ITokenService tokenService): IAuthService
    {
        public async Task<ServiceResult> AuthenticateAsync(SignInVm signInViewModel)
        {
            var tokenResponse = await tokenService.GetPasswordAccessToken(signInViewModel);
            if (tokenResponse?.Data == null || string.IsNullOrEmpty(tokenResponse.Data.AccessToken))
            {
                return ServiceResult.Error(
                    tokenResponse?.Data?.Error ?? "Auth error",
                    tokenResponse?.Data?.ErrorDescription ?? "Login failed"
                );
            }

            //Burası önemli cookie oluşturuyoruz.
            var userClaims = tokenService.ExtractClaims(tokenResponse.Data!.AccessToken!);
            var filteredClaims = userClaims
               .Where(c =>
                       c.Type == ClaimTypes.Name ||
                       c.Type == ClaimTypes.NameIdentifier ||
                           c.Type == ClaimTypes.Role)
               .ToList();
            var authenticationProperties = tokenService.CreateAuthenticationProperties(tokenResponse.Data!);

            var claimsIdentity = new ClaimsIdentity(filteredClaims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);


            await httpContextAccessor.HttpContext!
                .SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);


            return ServiceResult.Success();
        }
    }
}
