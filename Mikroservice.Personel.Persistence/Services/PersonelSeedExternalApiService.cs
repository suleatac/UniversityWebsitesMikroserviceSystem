using Microservice.Personel.Application.Contracts.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mikroservice.Personel.Persistence.Settings;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Mikroservice.Personel.Persistence.Services
{
    public class PersonelSeedExternalApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ExternalPersonelApiSettings _settings;
        private readonly ILogger<PersonelSeedExternalApiService> _logger;

        public PersonelSeedExternalApiService(
            HttpClient httpClient,
            IOptions<ExternalPersonelApiSettings> settings,
            ILogger<PersonelSeedExternalApiService> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;

            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
            _httpClient.Timeout = TimeSpan.FromMinutes(10);
        }
        public async Task<ApiResponseDto> GetPersonelsAsync(PersonelSyncRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", _settings.BasicAuthToken);

                var json = JsonSerializer.Serialize(request, new JsonSerializerOptions {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_settings.Endpoint, content, cancellationToken);

                var rawContent = await response.Content.ReadAsStringAsync(cancellationToken);

                return new ApiResponseDto(
                    response.IsSuccessStatusCode,
                    response.IsSuccessStatusCode ? null : $"HTTP {response.StatusCode}",
                    rawContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "External API çağrısı hatası");
                return new ApiResponseDto(false, ex.Message, string.Empty);
            }
        }
    }
}
