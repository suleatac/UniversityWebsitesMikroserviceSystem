using Microservice.Admin.Clients.SiteClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Site;
using Microsoft.EntityFrameworkCore;
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

        //Site paginated list
        public async Task<ServiceResult<PaginatedResult<SiteGetVm>>> GetSitesPaginatedAsync(
     int page, int pageSize, string? search, string? orderBy, string? orderDir)
        {
            _logger.LogInformation("API'den paginated site listesi çekiliyor. Page: {Page}, PageSize: {PageSize}", page, pageSize);

            var response = await _siteRefitService.GetSitesPaginatedAsync(page, pageSize, search, orderBy, orderDir);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}",
                    response.StatusCode, problemDetails?.Title);

                return ServiceResult<PaginatedResult<SiteGetVm>>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Paginated site listesi alınamadı");
            }

            _logger.LogInformation("Paginated site listesi başarıyla alındı. TotalCount: {TotalCount}",
                response.Content?.TotalCount);

            return ServiceResult<PaginatedResult<SiteGetVm>>.Success(response.Content!);
        }
        // GET BY ID
        public async Task<ServiceResult<SiteDetailGetVm>> GetSiteByIdAsync(int id)
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

                return ServiceResult<SiteDetailGetVm>
                    .Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Siteler alınamadı");

            }

            return ServiceResult<SiteDetailGetVm>.Success(response.Content!);
        }

        // CREATE
        public async Task<ServiceResult<object>> CreateSiteAsync(CreateSiteVm dto)
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

                return ServiceResult<object>
                    .Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Site oluşturulamadı");

            }

            _logger.LogInformation("Site başarıyla oluşturuldu. Id: {Id}", response.Content);
            return ServiceResult<object>.Success(true);
        }

        // UPDATE
        public async Task<ServiceResult<bool>> UpdateSiteAsync(SiteDetailGetVm dto)
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
