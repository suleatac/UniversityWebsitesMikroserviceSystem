using Duende.IdentityModel.Client;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.Settings;
using Microservice.Admin.ViewModels.UserRole;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class KeycloakRoleService : IKeycloakRoleService
    {
        private readonly IdentitySetting _settings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<KeycloakRoleService> _logger;
        private readonly ITokenService _tokenService;

        public KeycloakRoleService(
            IdentitySetting settings,
            IHttpClientFactory httpClientFactory,
            ILogger<KeycloakRoleService> logger,
            ITokenService tokenService)
        {
            _settings = settings;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Keycloak Admin API ile realm rollerini getirir.
        /// GET /admin/realms/{realm}/roles
        /// </summary>
        public async Task<ServiceResult<List<KeycloakRoleVm>>> GetRealmRolesAsync()
        {
            var tokenResult = await _tokenService.GetAdminTokenAsync();
            if (tokenResult.IsFail)
                return ServiceResult<List<KeycloakRoleVm>>.Error(tokenResult.Fail.Detail);

            var client = _httpClientFactory.CreateClient();
            client.SetBearerToken(tokenResult.Data!.AccessToken!);

            var rolesUrl = $"{_settings.BaseAddress}/admin/realms/{_settings.RealmName}/roles";
            var response = await client.GetAsync(rolesUrl);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Keycloak realm rolleri alınamadı. StatusCode: {StatusCode}, Content: {Content}",
                    response.StatusCode, errorContent);
                return ServiceResult<List<KeycloakRoleVm>>.Error("Realm rolleri alınamadı", errorContent);
            }

            var roles = await response.Content.ReadFromJsonAsync<List<KeycloakRoleVm>>();
            return ServiceResult<List<KeycloakRoleVm>>.Success(roles ?? new List<KeycloakRoleVm>());
        }

        /// <summary>
        /// Keycloak Admin API ile kullanıcının sahip olduğu realm rollerini getirir.
        /// GET /admin/realms/{realm}/users/{userId}/role-mappings/realm
        /// </summary>
        public async Task<ServiceResult<List<KeycloakRoleVm>>> GetUserRolesAsync(string userId)
        {
            var tokenResult = await _tokenService.GetAdminTokenAsync();
            if (tokenResult.IsFail)
                return ServiceResult<List<KeycloakRoleVm>>.Error(tokenResult.Fail.Detail);

            var client = _httpClientFactory.CreateClient();
            client.SetBearerToken(tokenResult.Data!.AccessToken!);

            var userRolesUrl = $"{_settings.AdminUserAddress}/{userId}/role-mappings/realm";
            var response = await client.GetAsync(userRolesUrl);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Kullanıcı rolleri alınamadı. UserId: {UserId}, StatusCode: {StatusCode}, Content: {Content}",
                    userId, response.StatusCode, errorContent);
                return ServiceResult<List<KeycloakRoleVm>>.Error("Kullanıcı rolleri alınamadı", errorContent);
            }

            var roles = await response.Content.ReadFromJsonAsync<List<KeycloakRoleVm>>();
            return ServiceResult<List<KeycloakRoleVm>>.Success(roles ?? new List<KeycloakRoleVm>());
        }

        /// <summary>
        /// Keycloak Admin API ile kullanıcıya realm rolü atar.
        /// POST /admin/realms/{realm}/users/{userId}/role-mappings/realm
        /// Body: [{ "id": "roleId", "name": "roleName" }]
        /// </summary>
        public async Task<ServiceResult> AssignRoleToUserAsync(string userId, string roleName)
        {
            var tokenResult = await _tokenService.GetAdminTokenAsync();
            if (tokenResult.IsFail)
                return tokenResult;

            var client = _httpClientFactory.CreateClient();
            client.SetBearerToken(tokenResult.Data!.AccessToken!);

            // Önce rol bilgisini al (id gerekli)
            var rolesResult = await GetRealmRolesAsync();
            if (rolesResult.IsFail)
                return ServiceResult.Error("Rol bilgisi alınamadı", rolesResult.Fail.Detail);

            var role = rolesResult.Data!.FirstOrDefault(r =>
                string.Equals(r.Name, roleName, StringComparison.OrdinalIgnoreCase));

            if (role == null)
                return ServiceResult.Error("Rol bulunamadı", $"'{roleName}' adlı rol Keycloak'ta bulunamadı.");

            var assignUrl = $"{_settings.AdminUserAddress}/{userId}/role-mappings/realm";
            var rolePayload = new[]
            {
                new { id = role.Id, name = role.Name }
            };

            var response = await client.PostAsJsonAsync(assignUrl, rolePayload);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Rol atama başarısız. UserId: {UserId}, Role: {RoleName}, StatusCode: {StatusCode}, Content: {Content}",
                    userId, roleName, response.StatusCode, errorContent);
                return ServiceResult.Error("Rol atama başarısız", errorContent);
            }

            _logger.LogInformation("Rol atandı. UserId: {UserId}, Role: {RoleName}", userId, roleName);
            return ServiceResult.Success();
        }

        /// <summary>
        /// Keycloak Admin API ile kullanıcıdan realm rolünü kaldırır.
        /// DELETE /admin/realms/{realm}/users/{userId}/role-mappings/realm
        /// Body: [{ "id": "roleId", "name": "roleName" }]
        /// </summary>
        public async Task<ServiceResult> RemoveRoleFromUserAsync(string userId, string roleName)
        {
            var tokenResult = await _tokenService.GetAdminTokenAsync();
            if (tokenResult.IsFail)
                return tokenResult;

            var client = _httpClientFactory.CreateClient();
            client.SetBearerToken(tokenResult.Data!.AccessToken!);

            // Önce rol bilgisini al (id gerekli)
            var rolesResult = await GetRealmRolesAsync();
            if (rolesResult.IsFail)
                return ServiceResult.Error("Rol bilgisi alınamadı", rolesResult.Fail.Detail);

            var role = rolesResult.Data!.FirstOrDefault(r =>
                string.Equals(r.Name, roleName, StringComparison.OrdinalIgnoreCase));

            if (role == null)
                return ServiceResult.Error("Rol bulunamadı", $"'{roleName}' adlı rol Keycloak'ta bulunamadı.");

            var removeUrl = $"{_settings.AdminUserAddress}/{userId}/role-mappings/realm";
            var rolePayload = new[]
            {
                new { id = role.Id, name = role.Name }
            };

            // Keycloak DELETE ile birlikte body gönderiyor
            var request = new HttpRequestMessage(HttpMethod.Delete, removeUrl)
            {
                Content = JsonContent.Create(rolePayload)
            };

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Rol kaldırma başarısız. UserId: {UserId}, Role: {RoleName}, StatusCode: {StatusCode}, Content: {Content}",
                    userId, roleName, response.StatusCode, errorContent);
                return ServiceResult.Error("Rol kaldırma başarısız", errorContent);
            }

            _logger.LogInformation("Rol kaldırıldı. UserId: {UserId}, Role: {RoleName}", userId, roleName);
            return ServiceResult.Success();
        }

        /// <summary>
        /// Kullanıcıya birden fazla realm rolü atar
        /// </summary>
        public async Task<ServiceResult> AssignRolesToUserAsync(string userId, List<string> roleNames)
        {
            if (roleNames == null || roleNames.Count == 0)
                return ServiceResult.Success();

            var errors = new List<string>();
            foreach (var roleName in roleNames)
            {
                var result = await AssignRoleToUserAsync(userId, roleName);
                if (!result.IsSuccess)
                {
                    errors.Add($"Rol '{roleName}' atanamadı: {result.Fail?.Detail}");
                }
            }

            if (errors.Count > 0)
                return ServiceResult.Error("Bazı roller atanamadı", string.Join("; ", errors));

            return ServiceResult.Success();
        }

        /// <summary>
        /// Kullanıcıdan birden fazla realm rolünü kaldırır
        /// </summary>
        public async Task<ServiceResult> RemoveRolesFromUserAsync(string userId, List<string> roleNames)
        {
            if (roleNames == null || roleNames.Count == 0)
                return ServiceResult.Success();

            var errors = new List<string>();
            foreach (var roleName in roleNames)
            {
                var result = await RemoveRoleFromUserAsync(userId, roleName);
                if (!result.IsSuccess)
                {
                    errors.Add($"Rol '{roleName}' kaldırılamadı: {result.Fail?.Detail}");
                }
            }

            if (errors.Count > 0)
                return ServiceResult.Error("Bazı roller kaldırılamadı", string.Join("; ", errors));

            return ServiceResult.Success();
        }

        /// <summary>
        /// Kullanıcının belirli bir role sahip olup olmadığını kontrol eder
        /// </summary>
        public async Task<bool> IsUserInRoleAsync(string userId, string roleName)
        {
            var result = await GetUserRolesAsync(userId);
            if (result.IsFail)
                return false;

            return result.Data!.Any(r =>
                string.Equals(r.Name, roleName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
