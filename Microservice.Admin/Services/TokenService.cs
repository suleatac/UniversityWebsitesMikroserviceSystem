using Duende.IdentityModel.Client;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.Settings;
using Microservice.Admin.ViewModels.SignIn;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Microservice.Admin.Services
{
    public class TokenService(
        IHttpClientFactory httpClientFactory,
        IdentitySetting identitySetting,
        IRedisCacheService redisCacheService
        ):ITokenService
    {

        public List<Claim> ExtractClaims(string accessToken)
        {

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);
            return jwtToken.Claims.ToList<Claim>();
        }
        public AuthenticationProperties CreateAuthenticationProperties(TokenResponse tokenResponse)
        {
            var expiresAt = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);

            var props = new AuthenticationProperties {
                IsPersistent = true,
                ExpiresUtc = expiresAt
            };

            var tokens = new List<AuthenticationToken>
            {
                new AuthenticationToken
                {
                  Name = "access_token",
                  Value = tokenResponse.AccessToken!
                },
                //new AuthenticationToken
                //{
                //  Name = "refresh_token",
                //  Value = tokenResponse.RefreshToken ?? ""
                //},
                new AuthenticationToken
                {
                  Name = "expires_at",
                  Value = expiresAt.ToString("o")
                }
            };

            props.StoreTokens(tokens);
            return props;
        }

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
                ClientId = identitySetting.WebAdmin.ClientId,
                ClientSecret = identitySetting.WebAdmin.ClientSecret,
                RefreshToken = refreshToken
            });

            if (tokenResponse.IsError)
                return ServiceResult<TokenResponse>.Error(tokenResponse.Error!);
            


            return ServiceResult<TokenResponse>.Success(tokenResponse);
        }

        public async Task<ServiceResult<TokenResponse>> GetClientCredentialsAccessToken()
        {
            var cacheKey = "auth:client:token";
            var lockKey = "auth:client:lock";

            // 1. Cache kontrol
            var cachedToken = await redisCacheService.GetAsync<TokenResponse>(cacheKey);
            if (cachedToken != null)
            {
                return ServiceResult<TokenResponse>.Success(cachedToken);
            }

            // 2. Lock almaya çalış
            var lockAcquired = await redisCacheService.AcquireLockAsync(lockKey, TimeSpan.FromSeconds(10));

            if (!lockAcquired)
            {
                // başka instance token alıyor → bekle
                await Task.Delay(500);

                var retryToken = await redisCacheService.GetAsync<TokenResponse>(cacheKey);
                if (retryToken != null)
                {
                    return ServiceResult<TokenResponse>.Success(retryToken);
                }
            }

            try
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
                        ClientId = identitySetting.WebAdmin.ClientId,
                        ClientSecret = identitySetting.WebAdmin.ClientSecret,
                        Scope = "api.read api.write"
                    });

                if (tokenResponse.IsError)
                    return ServiceResult<TokenResponse>.Error(tokenResponse.Error!);

                // 4. Cache’e yaz
                await redisCacheService.SetAsync(
                    cacheKey,
                    tokenResponse,
                    TimeSpan.FromSeconds(tokenResponse.ExpiresIn - 60)
                );

                return ServiceResult<TokenResponse>.Success(tokenResponse);
            }
            finally
            {
                // 5. Lock bırak
                await redisCacheService.ReleaseLockAsync(lockKey);
            }
        }

        public async Task<ServiceResult<TokenResponse>> GetPasswordAccessToken(SignInVm signInViewModel)
        {
            var client = httpClientFactory.CreateClient("AuthClient");

            var discovery = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest {
                Address = identitySetting.Address,
                Policy = { RequireHttps = false } // sadece dev
            });

            if (discovery.IsError)
                return ServiceResult<TokenResponse>.Error(discovery.Error!);

            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest {
                Address = discovery.TokenEndpoint,
                ClientId = identitySetting.WebAdmin.ClientId,
                ClientSecret = identitySetting.WebAdmin.ClientSecret,
                UserName = signInViewModel.Email!,
                Password = signInViewModel.Password,
                Scope = "openid profile email offline_access"
            });

            if (tokenResponse.IsError)
                return ServiceResult<TokenResponse>.Error(tokenResponse.Error!);

            return ServiceResult<TokenResponse>.Success(tokenResponse);
        }

        public async Task<ServiceResult<TokenResponse>> GetAdminTokenAsync()
        {
            var client = httpClientFactory.CreateClient();
            var discovery = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest {
                Address = identitySetting.Address,
                Policy = { RequireHttps = false }
            });

            if (discovery.IsError)
            {
                return ServiceResult<TokenResponse>.Error("Auth server not reachable");
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest {
                    Address = discovery.TokenEndpoint,
                    ClientId = identitySetting.Admin.ClientId,
                    ClientSecret = identitySetting.Admin.ClientSecret
                });

            if (tokenResponse.IsError)
            {
                return ServiceResult<TokenResponse>.Error("Token alınamadı");
            }

            return ServiceResult<TokenResponse>.Success(tokenResponse);
        }


    }
}
