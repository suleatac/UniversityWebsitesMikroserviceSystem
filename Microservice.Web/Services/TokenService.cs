using Duende.IdentityModel.Client;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.Settings;
using Microsoft.AspNetCore.Authentication;

namespace Microservice.Web.Services
{
    public class TokenService(
        IHttpClientFactory httpClientFactory,
        IdentitySetting identitySetting,
        IRedisCacheService redisCacheService
        ):ITokenService
    {

        public async Task<ServiceResult<TokenResponse>> GetNewAccessTokenByRefreshToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return ServiceResult<TokenResponse>.Error("Refresh token boş");

            var client = httpClientFactory.CreateClient("RefreshTokenClient");

            var discovery = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest {
                Address = identitySetting.Address,
                Policy = { RequireHttps = false } // sadece dev
            });

            if (discovery.IsError)
                return ServiceResult<TokenResponse>.Error(discovery.Error!);

            var tokenResponse = await client.RequestRefreshTokenAsync(new RefreshTokenRequest {
                Address = discovery.TokenEndpoint,
                ClientId = identitySetting.Web.ClientId,
                ClientSecret = identitySetting.Web.ClientSecret,
                RefreshToken = refreshToken
            });

            if (tokenResponse.IsError)
                return ServiceResult<TokenResponse>.Error(tokenResponse.Error!);
            


            return ServiceResult<TokenResponse>.Success(tokenResponse);
        }

        public async Task<ServiceResult<TokenResponse>> GetClientCredentialsAccessToken()
        {

                // 3. Token al
                var client = httpClientFactory.CreateClient();

                var discovery = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest {
                    Address = identitySetting.Address,
                    Policy = { RequireHttps = false }
                });

                if (discovery.IsError)
                    return ServiceResult<TokenResponse>.Error(discovery.Error!);

                var tokenResponse = await client.RequestClientCredentialsTokenAsync(
                    new ClientCredentialsTokenRequest {
                        Address = discovery.TokenEndpoint,
                        ClientId = identitySetting.Web.ClientId,
                        ClientSecret = identitySetting.Web.ClientSecret
                    });

                if (tokenResponse.IsError)
                    return ServiceResult<TokenResponse>.Error(tokenResponse.Error!);

           

                return ServiceResult<TokenResponse>.Success(tokenResponse);
           
        }


    }
}
