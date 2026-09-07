using Microservice.Web.Clients.EtkinlikClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Etkinlik;
using System.Text.Json;

namespace Microservice.Web.Services
{

    public class EtkinlikService : IEtkinlikService
    {
        private readonly IEtkinlikClientServices _etkinlikClient;
        private readonly ILogger<EtkinlikService> _logger;

        public EtkinlikService(IEtkinlikClientServices etkinlikClientServices, ILogger<EtkinlikService> logger)
        {
            _etkinlikClient = etkinlikClientServices ?? throw new ArgumentNullException(nameof(etkinlikClientServices));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<List<EtkinlikVm>>> GetEtkinliklerAsync(int siteId, int dilId)
        {
            _logger.LogInformation("API'den etkinlik listesi çekiliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);

            var response = await _etkinlikClient.GetEtkinliklerAsync(siteId, dilId);

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

                return ServiceResult<List<EtkinlikVm>>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Etkinlikler alınamadı"
                );
            }

            _logger.LogInformation("Etkinlik listesi başarıyla alındı. Count: {Count}", response.Content?.Count);
            return ServiceResult<List<EtkinlikVm>>.Success(response.Content!);
        }

        // GET BY ID
        public async Task<ServiceResult<EtkinlikVm>> GetEtkinlikByIdAsync(int id)
        {
            _logger.LogInformation("Etkinlik getiriliyor. Id: {Id}", id);

            var response = await _etkinlikClient.GetEtkinlikByIdAsync(id);

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

                return ServiceResult<EtkinlikVm>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Etkinlik alınamadı"
                );
            }

            return ServiceResult<EtkinlikVm>.Success(response.Content!);
        }
    }
}
