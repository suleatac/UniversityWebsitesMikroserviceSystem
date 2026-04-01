using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mikroservice.Ogrenci.Application.Contracts.DTOs;
using Mikroservice.Ogrenci.Application.Contracts.Services;
using Mikroservice.Ogrenci.Persistence.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Mikroservice.Ogrenci.Persistence.Services
{
    public class OgrenciExternalApiService : IOgrenciExternalApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ExternalOgrenciApiSettings _settings;
        private readonly ILogger<OgrenciExternalApiService> _logger;

        public OgrenciExternalApiService(
            HttpClient httpClient,
            IOptions<ExternalOgrenciApiSettings> settings,
            ILogger<OgrenciExternalApiService> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;

            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
            _httpClient.Timeout = TimeSpan.FromMinutes(10);
        }
        public async Task<ApiResponseDto> GetOgrencisAsync(OgrenciSyncRequest request, CancellationToken cancellationToken = default)
        {
            try
            {


                var json = JsonSerializer.Serialize(request, new JsonSerializerOptions {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var base64String = Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(_settings.BasicAuthToken));

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", base64String);

                _httpClient.Timeout = TimeSpan.FromMinutes(10);

                ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls |
                    SecurityProtocolType.Tls11 |
                    SecurityProtocolType.Tls12;

                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_settings.Endpoint, stringContent, cancellationToken);

                var rawContent = await response.Content.ReadAsStringAsync();


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
