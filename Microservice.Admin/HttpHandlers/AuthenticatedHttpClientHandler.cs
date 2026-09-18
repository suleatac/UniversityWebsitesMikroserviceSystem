using Duende.IdentityModel.Client;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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

            var accessToken = await GetStoredTokenAsync(httpContextAccessor.HttpContext!, AuthTokenKeys.AccessToken);

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new UnauthorizedAccessException("Access token is null or empty");
            }

            request.SetBearerToken(accessToken);
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
            {
                return response;
            }

            var refreshToken = await GetStoredTokenAsync(httpContextAccessor.HttpContext!, AuthTokenKeys.RefreshToken);
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new UnauthorizedAccessException("Refresh token is null or empty");
            }

            var tokenResponse = await tokenService.GetNewAccessTokenByRefreshToken(refreshToken);
            if (tokenResponse.IsFail)
            {
                throw new UnauthorizedAccessException("Failed to refresh access token. ");
            }

            // Cookie/ticket yalnızca yeni token'larla güncellenir.
            // Claim'ler tekrar yazılmaz; aksi halde cookie içeriği her token
            // yenilemesinde yeniden şişer ve gereksiz claim'ler kopyalanır.
            var authenticationProperties = tokenService.CreateAuthenticationProperties(tokenResponse.Data!);

            await httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                httpContextAccessor.HttpContext.User,
                authenticationProperties);

            request.SetBearerToken(tokenResponse.Data!.AccessToken!);
            return await base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Access/refresh token'ı okur. Token'lar Redis ticket store ile sunucu tarafında
        /// tutulduğu için önce AuthenticationProperties üzerinden denenir; geriye dönük
        /// uyumluluk için eski <c>StoreTokens</c> yaklaşımı da desteklenir.
        /// </summary>
        private static async Task<string?> GetStoredTokenAsync(HttpContext context, string tokenName)
        {
            var token = await context.GetTokenAsync(tokenName);

            if (!string.IsNullOrEmpty(token))
            {
                return token;
            }

            var authenticateResult = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (authenticateResult.Succeeded
                && authenticateResult.Properties != null
                && authenticateResult.Properties.Items.TryGetValue(tokenName, out var value))
            {
                return value;
            }

            return null;
        }
    }
}
