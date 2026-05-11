using Microservice.Admin.Clients.SiteOzellikleriClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.SiteOzellikleri;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class SiteOzellikleriService : ISiteOzellikleriService
    {
        private readonly ISiteOzellikleriClientServices _siteOzellikleriClient;
        private readonly ILogger<SiteOzellikleriService> _logger;

        public SiteOzellikleriService(ISiteOzellikleriClientServices siteOzellikleriClient, ILogger<SiteOzellikleriService> logger)
        {
            _siteOzellikleriClient = siteOzellikleriClient ?? throw new ArgumentNullException(nameof(siteOzellikleriClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<SiteOzellikleriVm>> GetSiteOzellikleriAsync(int siteId)
        {
            _logger.LogInformation("SiteOzellikleri çekiliyor. SiteId: {SiteId}", siteId);
            var response = await _siteOzellikleriClient.GetSiteOzellikleriAsync(siteId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<SiteOzellikleriVm>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "SiteOzellikleri alınamadı");
            }

            return ServiceResult<SiteOzellikleriVm>.Success(response.Content!);
        }

        public async Task<ServiceResult<object>> CreateSiteOzellikleriAsync(SiteOzellikleriVm dto)
        {
            _logger.LogInformation("Yeni SiteOzellikleri oluşturuluyor.");
            var response = await _siteOzellikleriClient.CreateSiteOzellikleriAsync(dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "SiteOzellikleri oluşturulamadı");
            }

            _logger.LogInformation("SiteOzellikleri oluşturuldu.");
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> UpdateSiteOzellikleriAsync(int id, SiteOzellikleriVm dto)
        {
            _logger.LogInformation("SiteOzellikleri güncelleniyor. Id: {Id}", id);
            var response = await _siteOzellikleriClient.UpdateSiteOzellikleriAsync(id, dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? $"SiteOzellikleri güncellenemedi. Id: {id}");
            }

            _logger.LogInformation("SiteOzellikleri güncellendi. Id: {Id}", id);
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> DeleteSiteOzellikleriAsync(int id)
        {
            _logger.LogWarning("SiteOzellikleri silme isteği alındı. Id: {Id}", id);
            var response = await _siteOzellikleriClient.DeleteSiteOzellikleriAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "SiteOzellikleri silinemedi");
            }

            _logger.LogInformation("SiteOzellikleri silindi. Id: {Id}", id);
            return ServiceResult<object>.Success(true);
        }
    }
}