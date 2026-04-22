using Duende.IdentityModel.Client;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.Settings;
using Microservice.Admin.ViewModels.User;

namespace Microservice.Admin.Services
{
   
    public record UserCreateRequest(string Username,
       bool Enabled,
       string FirstName,
       string LastName,
       string Email,
       List<Credential> Credentials);

    public record Credential(
        string Type,
        string Value,
        bool Temporary = false
        );

    public record KeycloakErrorResponse(string ErrorMessage);
    public class UserAddService
    {
        private readonly IdentitySetting _settings;
        private readonly HttpClient _client;
        private readonly ILogger<UserAddService> _logger;

        public UserAddService(
            IdentitySetting settings,
            HttpClient client,
            ILogger<UserAddService> logger)
        {
            _settings = settings;
            _client = client;
            _logger = logger;
        }

        public async Task<ServiceResult> CreateAccount(UserAddVm model)
        {
            var tokenResult = await GetAdminTokenAsync();
            if (tokenResult.IsFail)
                return tokenResult;

            _client.SetBearerToken(tokenResult.Data!);

            var request = CreateUserRequest(model);

            var response = await _client.PostAsJsonAsync(_settings.AdminUserAddAddress, request);

            if (response.IsSuccessStatusCode)
                return ServiceResult.Success();

            return await HandleErrorResponse(response);
        }

        private async Task<ServiceResult<string>> GetAdminTokenAsync()
        {
            var discovery = await _client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _settings.Address,
                Policy = { RequireHttps = false }
            });

            if (discovery.IsError)
            {
                _logger.LogError("Discovery error: {Error}", discovery.Error);
                return ServiceResult<string>.Error("Auth server not reachable");
            }

            var tokenResponse = await _client.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discovery.TokenEndpoint,
                    ClientId = _settings.Admin.ClientId,
                    ClientSecret = _settings.Admin.ClientSecret
                });

            if (tokenResponse.IsError)
            {
                _logger.LogError("Token error: {Error}", tokenResponse.Error);
                return ServiceResult<string>.Error("Token alınamadı");
            }

            return ServiceResult<string>.Success(tokenResponse.AccessToken!);
        }

        private static UserCreateRequest CreateUserRequest(UserAddVm model)
        {
            return new(
                model.UserName!,
                true,
                model.FirstName!,
                model.LastName!,
                model.Email!,
                new List<Credential>
                {
                new("password", model.Password!, false)
                });
        }

        private async Task<ServiceResult> HandleErrorResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            _logger.LogError("Keycloak Error: {StatusCode} - {Content}",
                response.StatusCode, content);

            try
            {
                var error = await response.Content.ReadFromJsonAsync<KeycloakErrorResponse>();

                return ServiceResult.Error(
                    "Kullanıcı oluşturulamadı",
                    error?.ErrorMessage ?? content);
            }
            catch
            {
                return ServiceResult.Error(
                    "Beklenmeyen hata",
                    content);
            }
        }
    }
}
