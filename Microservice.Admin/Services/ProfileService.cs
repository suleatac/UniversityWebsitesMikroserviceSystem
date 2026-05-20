using Duende.IdentityModel.Client;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.Settings;
using Microservice.Admin.ViewModels.Profile;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microservice.Admin.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IdentitySetting _settings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ProfileService> _logger;
        private readonly ITokenService _tokenService;

        public ProfileService(
            IdentitySetting settings,
            IHttpClientFactory httpClientFactory,
            ILogger<ProfileService> logger,
            ITokenService tokenService)
        {
            _settings = settings;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task<ServiceResult<ProfileVm>> GetCurrentUserProfileAsync(string keycloakUserId)
        {
            var tokenResult = await _tokenService.GetAdminTokenAsync();
            if (tokenResult.IsFail)
                return ServiceResult<ProfileVm>.Error(tokenResult.Fail.Detail);

            var client = _httpClientFactory.CreateClient();
            client.SetBearerToken(tokenResult.Data!.AccessToken!);

            var url = $"{_settings.AdminUserAddress}/{keycloakUserId}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Kullanıcı profili alınamadı. UserId: {UserId}, StatusCode: {StatusCode}, Content: {Content}",
                    keycloakUserId, response.StatusCode, errorContent);
                return ServiceResult<ProfileVm>.Error("Kullanıcı profili alınamadı");
            }

            var json = await response.Content.ReadAsStringAsync();
            var keycloakUser = JsonSerializer.Deserialize<KeycloakUserRepresentation>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (keycloakUser == null)
                return ServiceResult<ProfileVm>.Error("Kullanıcı profili parse edilemedi");

            var profile = new ProfileVm
            {
                Id = keycloakUser.Id ?? keycloakUserId,
                UserName = keycloakUser.Username ?? "",
                FirstName = keycloakUser.FirstName ?? "",
                LastName = keycloakUser.LastName ?? "",
                Email = keycloakUser.Email ?? "",
                Enabled = keycloakUser.Enabled,
                ProfileImageUrl = keycloakUser.Attributes?.TryGetValue("profileImage", out var images) == true
                    ? images.FirstOrDefault(): null
            };

            return ServiceResult<ProfileVm>.Success(profile);
        }

        public async Task<ServiceResult> UpdateUserProfileAsync(string keycloakUserId, ProfileVm model)
        {
            var tokenResult = await _tokenService.GetAdminTokenAsync();
            if (tokenResult.IsFail)
                return tokenResult;

            var client = _httpClientFactory.CreateClient();
            client.SetBearerToken(tokenResult.Data!.AccessToken!);

            // First, get the current user to preserve existing attributes
            var getUrl = $"{_settings.AdminUserAddress}/{keycloakUserId}";
            var getResponse = await client.GetAsync(getUrl);

            Dictionary<string, string[]> existingAttributes = new();
            if (getResponse.IsSuccessStatusCode)
            {
                var existingJson = await getResponse.Content.ReadAsStringAsync();
                var existingUser = JsonSerializer.Deserialize<KeycloakUserRepresentation>(existingJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (existingUser?.Attributes != null)
                    existingAttributes = existingUser.Attributes;
            }

            // Update profileImage attribute
            if (!string.IsNullOrWhiteSpace(model.ProfileImageUrl))
            {
                existingAttributes["profileImage"] = new[] { model.ProfileImageUrl };
            }
            else
            {
                existingAttributes.Remove("profileImage");
            }

            var updateRequest = new
            {
                firstName = model.FirstName,
                lastName = model.LastName,
                email = model.Email,
                enabled = model.Enabled,
                attributes = existingAttributes
            };

            var updateUrl = $"{_settings.AdminUserAddress}/{keycloakUserId}";
            var response = await client.PutAsJsonAsync(updateUrl, updateRequest);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Kullanıcı profili güncellendi. UserId: {UserId}", keycloakUserId);
                return ServiceResult.Success();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Profil güncellenemedi. UserId: {UserId}, StatusCode: {StatusCode}, Content: {Content}",
                keycloakUserId, response.StatusCode, errorContent);
            return ServiceResult.Error("Profil güncellenemedi", errorContent);
        }

        /// <summary>
        /// Keycloak User Representation for JSON deserialization
        /// </summary>
        private class KeycloakUserRepresentation
        {
            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("username")]
            public string? Username { get; set; }

            [JsonPropertyName("firstName")]
            public string? FirstName { get; set; }

            [JsonPropertyName("lastName")]
            public string? LastName { get; set; }

            [JsonPropertyName("email")]
            public string? Email { get; set; }

            [JsonPropertyName("enabled")]
            public bool Enabled { get; set; }

            [JsonPropertyName("attributes")]
            public Dictionary<string, string[]>? Attributes { get; set; }
        }
    }
}