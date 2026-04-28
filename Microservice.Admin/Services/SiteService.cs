using Microservice.Admin.Clients.SiteClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Site;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class SiteService : ISiteService
    {
        private readonly ISiteClientServices _siteRefitService;
        private readonly ILogger<SiteService> _logger;

        public SiteService(ISiteClientServices siteRefitService, ILogger<SiteService> logger)
        {
            _siteRefitService = siteRefitService ?? throw new ArgumentNullException(nameof(siteRefitService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        // LIST
        public async Task<ServiceResult<List<SiteGetVm>>> GetSitesAsync()
        {
            _logger.LogInformation("API'den site listesi çekiliyor.");

            var response = await _siteRefitService.GetSitesAsync();

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError(
                    "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode,
                    problemDetails?.Title,
                    problemDetails?.Detail
                );


                return ServiceResult<List<SiteGetVm>>.Error(
                problemDetails?.Detail ?? problemDetails?.Title ?? "Siteler alınamadı"
            );
            }

            _logger.LogInformation("Site listesi başarıyla alındı. Count: {Count}", response.Content?.Count);
            return ServiceResult<List<SiteGetVm>>.Success(response.Content!);
        }

        // GET BY ID
        public async Task<ServiceResult<SiteGetVm>> GetSiteByIdAsync(int id)
        {
            _logger.LogInformation("Site getiriliyor. Id: {Id}", id);

            var response = await _siteRefitService.GetSiteByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                   ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                   : null;

                _logger.LogError(
                    "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode,
                    problemDetails?.Title,
                    problemDetails?.Detail
                );

                return ServiceResult<SiteGetVm>
                    .Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Siteler alınamadı");

            }

            return ServiceResult<SiteGetVm>.Success(response.Content!);
        }

        // CREATE
        public async Task<ServiceResult<SiteGetVm>> CreateSiteAsync(CreateSiteVm dto)
        {
            _logger.LogInformation("Yeni site oluşturuluyor. Name: {Name}", dto.SiteAdi);

            var response = await _siteRefitService.CreateSiteAsync(dto);

            if (!response.IsSuccessStatusCode)
            {
                 var problemDetails = response.Error != null
                   ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                   : null;

                _logger.LogError(
                    "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode,
                    problemDetails?.Title,
                    problemDetails?.Detail
                );

                return ServiceResult<SiteGetVm>
                    .Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Site oluşturulamadı");

            }

            _logger.LogInformation("Site başarıyla oluşturuldu. Id: {Id}", response.Content?.Id);
            return ServiceResult<SiteGetVm>.Success(response.Content!);
        }

        // UPDATE
        public async Task<ServiceResult<bool>> UpdateSiteAsync(UpdateSiteVm dto)
        {
            _logger.LogInformation("Site güncelleniyor. Id: {Id}", dto.Id);

            var response = await _siteRefitService.UpdateSiteAsync(dto.Id, dto);

            if (!response.IsSuccessStatusCode)
            {


                var problemDetails = response.Error != null
                  ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                  : null;

                _logger.LogError(
                    "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode,
                    problemDetails?.Title,
                    problemDetails?.Detail
                );

                return ServiceResult<bool>
                    .Error(problemDetails?.Detail ?? problemDetails?.Title ?? $"Site güncellenemedi. Id: {dto.Id}");

            }

            _logger.LogInformation("Site güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<bool>.Success(true);
        }

        // DELETE
        public async Task<ServiceResult<bool>> DeleteSiteAsync(int id)
        {
            _logger.LogWarning("Site siliniyor. Id: {Id}", id);

            var response = await _siteRefitService.DeleteSiteAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                 ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                 : null;

                _logger.LogError(
                    "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode,
                    problemDetails?.Title,
                    problemDetails?.Detail
                );

                return ServiceResult<bool>
                    .Error(problemDetails?.Detail ?? problemDetails?.Title ?? $"Site silinemedi. Id: {id}");

            }

            _logger.LogInformation("Site silindi. Id: {Id}", id);
            return ServiceResult<bool>.Success(true);
        }

       
    }
}
