using Duende.IdentityModel.Client;
using Microservice.Admin.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;

namespace Microservice.Admin.HttpHandlers
{
    public class AuthenticatedHttpClientHandler(IHttpContextAccessor httpContextAccessor, ITokenService tokenService) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //eğer null ise demekki bi request gelmemiş demektir. bu gelen backgroundanmış.
            if (httpContextAccessor.HttpContext == null)
            {
                return await base.SendAsync(request, cancellationToken);
            }


            if (!httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new UnauthorizedAccessException("Access token is null or empty");
            }
            request.SetBearerToken(accessToken);
            //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);  yukarıdaki satırın yerine bunu da yapabiliriz
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
            {
                return response;
            }

            var refreshToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new UnauthorizedAccessException("Refresh token is null or empty");
            }


            var tokenResponse = await tokenService.GetNewAccessTokenByRefreshToken(refreshToken);
            if (tokenResponse.IsFail)
            {
                throw new UnauthorizedAccessException("Failed to refresh access token. ");
            }

            //Cookie güncelleme işlemi yapıldı.


            var authenticationProperties = tokenService.CreateAuthenticationProperties(tokenResponse.Data!);
            var userClaim = httpContextAccessor.HttpContext.User.Claims;

            var claimIdentity = new ClaimsIdentity(userClaim, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);




            request.SetBearerToken(tokenResponse.Data!.AccessToken!);
            return await base.SendAsync(request, cancellationToken);








        }

    }
}
