using Duende.IdentityModel.Client;
using Microservice.Web.Services.ServiceResults;

namespace Microservice.Web.Services.Interfaces
{
    public interface ITokenService
    {

        Task<ServiceResult<TokenResponse>> GetNewAccessTokenByRefreshToken(string refreshToken);
        Task<ServiceResult<TokenResponse>> GetClientCredentialsAccessToken();

    }
}
