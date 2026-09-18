using Duende.IdentityModel.Client;
using Microservice.Admin.Services.Interfaces;

namespace Microservice.Admin.HttpHandlers
{
    public class ClientAuthenticatedHttpClientHandler(IHttpContextAccessor httpContextAccessor, ITokenService tokenService) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Kullanıcı kimliği doğrulanmışsa token'ı AuthenticatedHttpClientHandler ekler,
            // bu handler yalnızca anonim/arka plan çağrıları için client credentials kullanır.
            if (httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var tokenResult = await tokenService.GetClientCredentialsAccessToken();

            if (tokenResult?.IsSuccess == true && !string.IsNullOrEmpty(tokenResult.Data?.AccessToken))
            {
                request.SetBearerToken(tokenResult.Data.AccessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
