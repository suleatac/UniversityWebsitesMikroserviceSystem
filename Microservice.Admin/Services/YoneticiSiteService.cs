using Microservice.Admin.Clients.YoneticiSiteClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.YoneticiSite;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class YoneticiSiteService : IYoneticiSiteService
    {
        private readonly IYoneticiSiteClientServices _yoneticiSiteClient;
        private readonly ILogger<YoneticiSiteService> _logger;

        public YoneticiSiteService(IYoneticiSiteClientServices yoneticiSiteClient, ILogger<YoneticiSiteService> logger)
        {
            _yoneticiSiteClient = yoneticiSiteClient ?? throw new ArgumentNullException(nameof(yoneticiSiteClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<List<GetYoneticiSiteVm>>> GetYoneticiSitesAsync(int siteId)
        {
            _logger.LogInformation("YoneticiSite listesi çekiliyor. SiteId: {SiteId}", siteId);
            var response = await _yoneticiSiteClient.GetYoneticiSitesAsync(siteId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<List<GetYoneticiSiteVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "YoneticiSite listesi alınamadı");
            }

            return ServiceResult<List<GetYoneticiSiteVm>>.Success(response.Content!);
        }

        public async Task<ServiceResult<YoneticiSiteDetailVm>> GetYoneticiSiteByIdAsync(int id)
        {
            _logger.LogInformation("YoneticiSite getiriliyor. Id: {Id}", id);
            var response = await _yoneticiSiteClient.GetYoneticiSiteByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<YoneticiSiteDetailVm>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "YoneticiSite bulunamadı");
            }

            return ServiceResult<YoneticiSiteDetailVm>.Success(response.Content!);
        }

        public async Task<ServiceResult<object>> CreateYoneticiSiteAsync(CreateYoneticiSiteVm dto)
        {
            _logger.LogInformation("Yeni YoneticiSite oluşturuluyor.");
            var response = await _yoneticiSiteClient.CreateYoneticiSiteAsync(dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "YoneticiSite oluşturulamadı");
            }

            _logger.LogInformation("YoneticiSite oluşturuldu.");
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> UpdateYoneticiSiteAsync(YoneticiSiteDetailVm dto)
        {
            _logger.LogInformation("YoneticiSite güncelleniyor. Id: {Id}", dto.Id);
            var response = await _yoneticiSiteClient.UpdateYoneticiSiteAsync(dto.Id, dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? $"YoneticiSite güncellenemedi. Id: {dto.Id}");
            }

            _logger.LogInformation("YoneticiSite güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> DeleteYoneticiSiteAsync(int id)
        {
            _logger.LogWarning("YoneticiSite silme isteği alındı. Id: {Id}", id);
            var response = await _yoneticiSiteClient.DeleteYoneticiSiteAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "YoneticiSite silinemedi");
            }

            _logger.LogInformation("YoneticiSite silindi. Id: {Id}", id);
            return ServiceResult<object>.Success(true);
        }
    }
}