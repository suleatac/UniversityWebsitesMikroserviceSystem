using Microservice.Web.Clients.SiteClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Site;
using System.Text.Json;

namespace Microservice.Web.Services
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

        public async Task<ServiceResult<SiteDetailGetVm>> GetSiteByHostAsync(string host)
        {
            _logger.LogInformation("Host'a göre site getiriliyor. Host: {Host}", host);

            var response = await _siteRefitService.GetSiteByHostAsync(host);

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
                    .Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Host için site bulunamadı");
            }

            return ServiceResult<SiteDetailGetVm>.Success(response.Content!);
        }


       

    }
}
