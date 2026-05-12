using Duende.IdentityModel.Client;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.Settings;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.User;

namespace Microservice.Admin.Services
{



    public record Credential(
        string Type,
        string Value,
        bool Temporary = false
        );

    public record KeycloakErrorResponse(string ErrorMessage);

    public class UserService : IUserService
    {
        private readonly IdentitySetting _settings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<UserService> _logger;
        private readonly ITokenService _tokenService;

        public UserService(
            IdentitySetting settings,
            IHttpClientFactory httpClientFactory,
            ILogger<UserService> logger,
            ITokenService tokenService)
        {
            _settings = settings;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task<ServiceResult<string>> CreateAccount(UserAddVm model)
        {
            var tokenResult = await _tokenService.GetAdminTokenAsync();
            if (tokenResult.IsFail)
                return ServiceResult<string>.Error(tokenResult.Fail.Detail);

            var client = _httpClientFactory.CreateClient();
            client.SetBearerToken(tokenResult.Data!.AccessToken!);

            var request = CreateUserRequest(model);

            var response = await client.PostAsJsonAsync(_settings.AdminUserAddress, request);

            if (response.IsSuccessStatusCode)
            {
                // Keycloak returns the created user's ID in the Location header
                // Location format: .../users/{userId}
                var location = response.Headers.Location;
                if (location != null)
                {
                    var userId = location.Segments.Last().Trim('/');
                    return ServiceResult<string>.Success(userId);
                }

                // Fallback: if Location header is not available, search by username
                var searchResult = await GetUserByUsernameAsync(model.Username!);
                if (searchResult.IsSuccess && searchResult.Data != null)
                {
                    return ServiceResult<string>.Success(searchResult.Data.Id);
                }

                return ServiceResult<string>.Error("Kullanıcı oluşturuldu ancak ID alınamadı");
            }

            return await HandleCreateErrorResponse(response);
        }

        private async Task<ServiceResult<UserListVm>> GetUserByUsernameAsync(string username)
        {
            var tokenResult = await _tokenService.GetAdminTokenAsync();
            if (tokenResult.IsFail)
                return ServiceResult<UserListVm>.Error(tokenResult.Fail.Detail);

            var client = _httpClientFactory.CreateClient();
            client.SetBearerToken(tokenResult.Data!.AccessToken!);

            var url = $"{_settings.AdminUserAddress}?username={Uri.EscapeDataString(username)}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return ServiceResult<UserListVm>.Error("Kullanıcı aranamadı");

            var users = await response.Content.ReadFromJsonAsync<List<UserListVm>>();
            var user = users?.FirstOrDefault(u =>
                string.Equals(u.UserName, username, StringComparison.OrdinalIgnoreCase));

            return user != null
                ? ServiceResult<UserListVm>.Success(user)
                : ServiceResult<UserListVm>.Error("Kullanıcı bulunamadı");
        }

        private async Task<ServiceResult<string>> HandleCreateErrorResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            _logger.LogError("Keycloak Error: {StatusCode} - {Content}",
                response.StatusCode, content);

            try
            {
                var error = await response.Content.ReadFromJsonAsync<KeycloakErrorResponse>();

                return ServiceResult<string>.Error(
                    "Kullanıcı işlemi başarısız",
                    error?.ErrorMessage ?? content);
            }
            catch
            {
                return ServiceResult<string>.Error(
                    "Beklenmeyen hata",
                    content);
            }
        }

        private static KeycloakUserCreateRequest CreateUserRequest(UserAddVm model)
        {


            var keycloakRequestModel = new KeycloakUserCreateRequest{
                Username = model.Username,
                Enabled = model.Enabled,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Attributes = new Dictionary<string, string[]>
   {
                   { "personId", new[] { model.PersonId.ToString() } }
                 },
                Credentials = new List<KeycloakCredential>
   {
                      new()
                       {
                         Type = "password",
                         Value = model.Password,
                         Temporary = false
                        }
                 }
            };

            return keycloakRequestModel;
        }

        // Kullanıcı silme
        public async Task<ServiceResult> DeleteAccount(string userId)
        {
            var tokenResult = await _tokenService.GetAdminTokenAsync();
            if (tokenResult.IsFail)
                return tokenResult;

            var client = _httpClientFactory.CreateClient();
            client.SetBearerToken(tokenResult.Data!.AccessToken!);

            var deleteUrl = $"{_settings.AdminUserAddress}/{userId}";
            var response = await client.DeleteAsync(deleteUrl);

            if (response.IsSuccessStatusCode)
                return ServiceResult.Success();

            return await HandleErrorResponse(response);
        }

        // Kullanıcı güncelleme
        public async Task<ServiceResult> UpdateAccount(string userId, UserUpdateVm model)
        {
            var tokenResult = await _tokenService.GetAdminTokenAsync();
            if (tokenResult.IsFail)
                return tokenResult;

            var client = _httpClientFactory.CreateClient();
            client.SetBearerToken(tokenResult.Data!.AccessToken!);

            var updateUrl = $"{_settings.AdminUserAddress}/{userId}";
            var request = new
            {
                firstName = model.FirstName,
                lastName = model.LastName,
                email = model.Email,
                enabled = model.Enabled
            };

            var response = await client.PutAsJsonAsync(updateUrl, request);

            if (response.IsSuccessStatusCode)
                return ServiceResult.Success();

            return await HandleErrorResponse(response);
        }

        // Kullanıcı listeleme
        public async Task<ServiceResult<List<UserListVm>>> GetUsersAsync()
        {
            var tokenResult = await _tokenService.GetAdminTokenAsync();
            if (tokenResult.IsFail)
                return ServiceResult<List<UserListVm>>.Error(tokenResult.Fail.Detail);

            var client = _httpClientFactory.CreateClient();
            client.SetBearerToken(tokenResult.Data!.AccessToken!);

            var response = await client.GetAsync(_settings.AdminUserAddress);

            if (!response.IsSuccessStatusCode)
                return ServiceResult<List<UserListVm>>.Error("Kullanıcılar alınamadı");

            var users = await response.Content.ReadFromJsonAsync<List<UserListVm>>();
            return ServiceResult<List<UserListVm>>.Success(users!);
        }

        // Kullanıcı Id bilgisine göre alma
        public async Task<ServiceResult<UserListVm>> GetUserByIdAsync(string userId)
        {
            var tokenResult = await _tokenService.GetAdminTokenAsync();
            if (tokenResult.IsFail)
                return ServiceResult<UserListVm>.Error(tokenResult.Fail.Detail);

            var client = _httpClientFactory.CreateClient();
            client.SetBearerToken(tokenResult.Data!.AccessToken!);

            var url = $"{_settings.AdminUserAddress}/{userId}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return ServiceResult<UserListVm>.Error("Kullanıcı bulunamadı");

            var user = await response.Content.ReadFromJsonAsync<UserListVm>();
            return ServiceResult<UserListVm>.Success(user!);
        }

        // Sayfalama ile kullanıcı alma
        public async Task<ServiceResult<PaginatedResult<UserListVm>>> GetUsersPaginatedAsync(
            int page, int pageSize, string search, string orderBy, string orderDir)
        {
            var tokenResult = await _tokenService.GetAdminTokenAsync();
            if (tokenResult.IsFail)
                return ServiceResult<PaginatedResult<UserListVm>>.Error(tokenResult.Fail.Detail);

            var client = _httpClientFactory.CreateClient();
            client.SetBearerToken(tokenResult.Data!.AccessToken!);

            // Keycloak toplam kullanıcı sayısını al
            var countUrl = $"{_settings.AdminUserAddress}/count";
            var countResponse = await client.GetAsync(countUrl);
            int totalCount = 0;

            if (countResponse.IsSuccessStatusCode)
            {
                var countContent = await countResponse.Content.ReadAsStringAsync();
                int.TryParse(countContent, out totalCount);
            }

            // Sayfalama parametreleri ile kullanıcıları al
            int first = (page - 1) * pageSize;
            var url = $"{_settings.AdminUserAddress}?first={first}&max={pageSize}";

            if (!string.IsNullOrWhiteSpace(search))
                url += $"&search={Uri.EscapeDataString(search)}";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return ServiceResult<PaginatedResult<UserListVm>>.Error("Kullanıcılar alınamadı");

            var users = await response.Content.ReadFromJsonAsync<List<UserListVm>>() ?? new List<UserListVm>();

            // İstemci tarafı sıralama
            users = (orderBy?.ToLower(), orderDir?.ToLower()) switch
            {
                ("username", "asc") => users.OrderBy(x => x.UserName).ToList(),
                ("username", "desc") => users.OrderByDescending(x => x.UserName).ToList(),
                ("firstname", "asc") => users.OrderBy(x => x.FirstName).ToList(),
                ("firstname", "desc") => users.OrderByDescending(x => x.FirstName).ToList(),
                ("lastname", "asc") => users.OrderBy(x => x.LastName).ToList(),
                ("lastname", "desc") => users.OrderByDescending(x => x.LastName).ToList(),
                ("email", "asc") => users.OrderBy(x => x.Email).ToList(),
                ("email", "desc") => users.OrderByDescending(x => x.Email).ToList(),
                _ => users.OrderByDescending(x => x.UserName).ToList()
            };

            var paginated = new PaginatedResult<UserListVm>
            {
                Data = users,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return ServiceResult<PaginatedResult<UserListVm>>.Success(paginated);
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
                    "Kullanıcı işlemi başarısız",
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
