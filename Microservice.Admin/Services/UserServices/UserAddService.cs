using Duende.IdentityModel.Client;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.Settings;
using Microservice.Admin.ViewModels.User;

namespace Microservice.Admin.Services.UserServices
{

    public record UserCreateRequest(string Username,
       bool Enabled,
       string FirstName,
       string LastName,
       string Email,
       int PersonId, // yeni alan
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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<UserAddService> _logger;
        private readonly TokenService _tokenService;
        public UserAddService(
            IdentitySetting settings,
            IHttpClientFactory httpClientFactory,
            ILogger<UserAddService> logger,
            TokenService tokenService)
        {
            _settings = settings;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task<ServiceResult> CreateAccount(UserAddVm model)
        {
            var tokenResult = await _tokenService.GetAdminTokenAsync();
            if (tokenResult.IsFail)
                return tokenResult;

            var client = _httpClientFactory.CreateClient();
            client.SetBearerToken(tokenResult.Data!);

            var request = CreateUserRequest(model);

            var response = await client.PostAsJsonAsync(_settings.AdminUserAddress, request);

            if (response.IsSuccessStatusCode)
                return ServiceResult.Success();

            return await HandleErrorResponse(response);
        }



        private static UserCreateRequest CreateUserRequest(UserAddVm model)
        {
            return new(
                model.UserName!,
                true,
                model.FirstName!,
                model.LastName!,
                model.Email!,
                model.PersonId,
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
