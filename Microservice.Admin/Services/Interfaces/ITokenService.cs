using Duende.IdentityModel.Client;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.SignIn;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Microservice.Admin.Services.Interfaces
{
    public interface ITokenService
    {
        List<Claim> ExtractClaims(string accessToken);
        AuthenticationProperties CreateAuthenticationProperties(TokenResponse tokenResponse);
        Task<ServiceResult<TokenResponse>> GetNewAccessTokenByRefreshToken(string refreshToken);
        Task<ServiceResult<TokenResponse>> GetClientCredentialsAccessToken();
        Task<ServiceResult<TokenResponse>> GetPasswordAccessToken(SignInVm signInViewModel);
        Task<ServiceResult<TokenResponse>> GetAdminTokenAsync();
    }
}
