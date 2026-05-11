using Microservice.Admin.Clients.SitePersonelClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.SitePersonel;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class SitePersonelService : ISitePersonelService
    {
        private readonly ISitePersonelClientServices _sitePersonelClient;
        private readonly ILogger<SitePersonelService> _logger;

        public SitePersonelService(ISitePersonelClientServices sitePersonelClient, ILogger<SitePersonelService> logger)
        {
            _sitePersonelClient = sitePersonelClient ?? throw new ArgumentNullException(nameof(sitePersonelClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<List<GetSitePersonelVm>>> GetSitePersonellerAsync(int siteId)
        {
            _logger.LogInformation("Site personel listesi çekiliyor. SiteId: {SiteId}", siteId);
            var response = await _sitePersonelClient.GetSitePersonellerAsync(siteId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<List<GetSitePersonelVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Site personelleri alınamadı");
            }

            return ServiceResult<List<GetSitePersonelVm>>.Success(response.Content!);
        }

        public async Task<ServiceResult<SitePersonelDetailVm>> GetSitePersonelByIdAsync(int id)
        {
            _logger.LogInformation("Site personel getiriliyor. Id: {Id}", id);
            var response = await _sitePersonelClient.GetSitePersonelByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<SitePersonelDetailVm>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Site personel bulunamadı");
            }

            return ServiceResult<SitePersonelDetailVm>.Success(response.Content!);
        }

        public async Task<ServiceResult<object>> CreateSitePersonelAsync(CreateSitePersonelVm dto)
        {
            _logger.LogInformation("Yeni site personel oluşturuluyor.");
            var response = await _sitePersonelClient.CreateSitePersonelAsync(dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Site personel oluşturulamadı");
            }

            _logger.LogInformation("Site personel oluşturuldu.");
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> UpdateSitePersonelAsync(SitePersonelDetailVm dto)
        {
            _logger.LogInformation("Site personel güncelleniyor. Id: {Id}", dto.Id);
            var response = await _sitePersonelClient.UpdateSitePersonelAsync(dto.Id, dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? $"Site personel güncellenemedi. Id: {dto.Id}");
            }

            _logger.LogInformation("Site personel güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> DeleteSitePersonelAsync(int id)
        {
            _logger.LogWarning("Site personel silme isteği alındı. Id: {Id}", id);
            var response = await _sitePersonelClient.DeleteSitePersonelAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Site personel silinemedi");
            }

            _logger.LogInformation("Site personel silindi. Id: {Id}", id);
            return ServiceResult<object>.Success(true);
        }
    }
}