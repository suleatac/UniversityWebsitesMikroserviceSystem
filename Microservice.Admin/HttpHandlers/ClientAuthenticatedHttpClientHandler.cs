using Duende.IdentityModel.Client;
using Microservice.Admin.Services.Interfaces;

namespace Microservice.Admin.HttpHandlers
{
    public class ClientAuthenticatedHttpClientHandler(IHttpContextAccessor httpContextAccessor, ITokenService tokenService) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            //eğer null ise demekki bi request gelmemiş demektir..
            if (httpContextAccessor.HttpContext == null)
                return await base.SendAsync(request, cancellationToken);
           
            if (httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated)
                return await base.SendAsync(request, cancellationToken);
            

            var tokenResponse = await tokenService.GetClientCredentialsAccessToken();

            if (tokenResponse.IsFail)
                throw new UnauthorizedAccessException($"Client Token request failed:{tokenResponse.Data!.Error}");
            
            request.SetBearerToken(tokenResponse.Data!.AccessToken!);
            return await base.SendAsync(request, cancellationToken);


        }
    }
}
