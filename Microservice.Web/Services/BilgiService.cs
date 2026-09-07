using Microservice.Web.Clients.BilgiClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Bilgi;
using System.Text.Json;

namespace Microservice.Web.Services
{
    public class BilgiService : IBilgiService
    {
        private readonly IBilgiClientServices _bilgiClient;
        private readonly ILogger<BilgiService> _logger;

        public BilgiService(IBilgiClientServices bilgiClientServices, ILogger<BilgiService> logger)
        {
            _bilgiClient = bilgiClientServices ?? throw new ArgumentNullException(nameof(bilgiClientServices));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<List<BilgiVm>>> GetBilgisAsync(int siteId, int dilId)
        {
            _logger.LogInformation("API'den bilgi listesi çekiliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);

            var response = await _bilgiClient.GetBilgisAsync(siteId, dilId);

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

                return ServiceResult<List<BilgiVm>>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Bilgiler alınamadı"
                );
            }

            _logger.LogInformation("Bilgi listesi başarıyla alındı. Count: {Count}", response.Content?.Count);
            return ServiceResult<List<BilgiVm>>.Success(response.Content!);
        }

        // GET BY ID
        public async Task<ServiceResult<BilgiVm>> GetBilgiByIdAsync(int id)
        {
            _logger.LogInformation("Bilgi getiriliyor. Id: {Id}", id);

            var response = await _bilgiClient.GetBilgiByIdAsync(id);

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

                return ServiceResult<BilgiVm>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Bilgi alınamadı"
                );
            }

            return ServiceResult<BilgiVm>.Success(response.Content!);
        }
    }
}
