using Microservice.Web.Clients.HaberClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Haber;
using System.Text.Json;

namespace Microservice.Web.Services
{
    public class HaberService : IHaberService
    {
        private readonly IHaberClientServices _haberClient;
        private readonly ILogger<HaberService> _logger;

        public HaberService(IHaberClientServices haberClient, ILogger<HaberService> logger)
        {
            _haberClient = haberClient ?? throw new ArgumentNullException(nameof(haberClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // LIST
        public async Task<ServiceResult<List<GetHaberVm>>> GetHabersAsync(int siteId, int dilId)
        {
            _logger.LogInformation("Haber listesi çekiliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);

            var response = await _haberClient.GetHabersAsync(siteId, dilId);

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

                return ServiceResult<List<GetHaberVm>>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Haberler alınamadı"
                );
            }

            _logger.LogInformation("Haber listesi alındı. Count: {Count}", response.Content?.Count);
            return ServiceResult<List<GetHaberVm>>.Success(response.Content!);
        }


        // GET BY ID
        public async Task<ServiceResult<HaberDetailVm>> GetHaberByIdAsync(int id)
        {
            _logger.LogInformation("Haber getiriliyor. Id: {Id}", id);

            var response = await _haberClient.GetHaberByIdAsync(id);

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

                return ServiceResult<HaberDetailVm>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Haber bulunamadı"
                );
            }

            return ServiceResult<HaberDetailVm>.Success(response.Content!);
        }

   

    }
}
