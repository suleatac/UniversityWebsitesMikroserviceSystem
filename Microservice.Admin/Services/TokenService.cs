using Duende.IdentityModel.Client;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.Settings;
using Microservice.Admin.ViewModels.SignIn;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Microservice.Admin.Services
{
    public class TokenService(IHttpClientFactory httpClientFactory, IdentitySetting identitySetting)
    {

        public List<Claim> ExtractClaims(string accessToken)
        {

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);
            return jwtToken.Claims.ToList<Claim>();
        }
        public AuthenticationProperties CreateAuthenticationProperties(TokenResponse tokenResponse)
        {
            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(tokenResponse.ExpiresIn)
            };
            authenticationProperties.StoreTokens(new List<AuthenticationToken>
            {
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = tokenResponse.AccessToken!
                },
                new AuthenticationToken
                {
                    Name =  OpenIdConnectParameterNames.RefreshToken,
                    Value = tokenResponse.RefreshToken!
                },
                new AuthenticationToken
                {
                    Name =  OpenIdConnectParameterNames.ExpiresIn,
                    Value = authenticationProperties.ExpiresUtc?.ToString("o")!
                }




            });
            return authenticationProperties;
        }

        public async Task<TokenResponse> GetNewAccessTokenByRefreshToken(string refreshToken)
        {



            var discoveryRequest = new DiscoveryDocumentRequest()
            {
                Address = identitySetting.Address,
                Policy =
               {
                    RequireHttps=false
                }
            };
            var httpClient = httpClientFactory.CreateClient("GetNewAccessTokenByRefreshToken");
            httpClient.BaseAddress = new Uri(identitySetting.Address);
            var discoveryResponse = await httpClient.GetDiscoveryDocumentAsync(discoveryRequest);

            if (discoveryResponse.IsError)
            {
                throw new Exception(discoveryResponse.Error);
            }


            var tokenResponse = await httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,
                ClientId = identitySetting.WebAdmin.ClientId,
                ClientSecret = identitySetting.WebAdmin.ClientSecret,
                RefreshToken = refreshToken
            });

            return tokenResponse;


        }

        public async Task<TokenResponse> GetClientCredentialsAccessToken()
        {
            var discoveryRequest = new DiscoveryDocumentRequest()
            {
                Address = identitySetting.Address,
                Policy =
               {
                    RequireHttps=false
                }
            };
            var httpClient = httpClientFactory.CreateClient("GetClientAccessToken");
            httpClient.BaseAddress = new Uri(identitySetting.Address);
            var discoveryResponse = await httpClient.GetDiscoveryDocumentAsync(discoveryRequest);
            if (discoveryResponse.IsError)
            {
                throw new Exception(discoveryResponse.Error);
            }


            var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,
                ClientId = identitySetting.WebAdmin.ClientId,
                ClientSecret = identitySetting.WebAdmin.ClientSecret
            });
            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }
            return tokenResponse;
        }


        public async Task<TokenResponse> GetPasswordAccessToken(SignInVm signInViewModel)
        {
            var discoveryRequest = new DiscoveryDocumentRequest()
            {
                Address = identitySetting.Address,
                Policy =
                {
                    RequireHttps=false
                }
            };
            var httpPasswordClient = httpClientFactory.CreateClient("GetPasswordAccessToken");
            httpPasswordClient.BaseAddress = new Uri(identitySetting.Address);
            var discoveryResponse = await httpPasswordClient.GetDiscoveryDocumentAsync(discoveryRequest);

            if (discoveryResponse.IsError)
            {
                throw new Exception(discoveryResponse.Error);
            }



            var tokenResponse = await httpPasswordClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,
                ClientId = identitySetting.WebAdmin.ClientId,
                ClientSecret = identitySetting.WebAdmin.ClientSecret,
                UserName = signInViewModel.Email!,
                Password = signInViewModel.Password,
                Scope = "offline_access"
            });





            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }


            return tokenResponse;
        }


        public async Task<ServiceResult<string>> GetAdminTokenAsync()
        {
            var client = httpClientFactory.CreateClient();
            var discovery = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = identitySetting.Address,
                Policy = { RequireHttps = false }
            });

            if (discovery.IsError)
            {
                return ServiceResult<string>.Error("Auth server not reachable");
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discovery.TokenEndpoint,
                    ClientId = identitySetting.Admin.ClientId,
                    ClientSecret = identitySetting.Admin.ClientSecret
                });

            if (tokenResponse.IsError)
            {
                return ServiceResult<string>.Error("Token alınamadı");
            }

            return ServiceResult<string>.Success(tokenResponse.AccessToken!);
        }


    }
}
