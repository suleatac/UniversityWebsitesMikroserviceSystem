using Microservice.Admin.Clients.YoneticiTipiClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.YoneticiTipi;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class YoneticiTipiService : IYoneticiTipiService
    {
        private readonly IYoneticiTipiClientServices _yoneticiTipiClient;
        private readonly ILogger<YoneticiTipiService> _logger;

        public YoneticiTipiService(IYoneticiTipiClientServices yoneticiTipiClient, ILogger<YoneticiTipiService> logger)
        {
            _yoneticiTipiClient = yoneticiTipiClient ?? throw new ArgumentNullException(nameof(yoneticiTipiClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<List<GetYoneticiTipiVm>>> GetYoneticiTipleriAsync()
        {
            _logger.LogInformation("Yonetici tipleri çekiliyor.");
            var response = await _yoneticiTipiClient.GetYoneticiTipleriAsync();

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<List<GetYoneticiTipiVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Yonetici tipleri alınamadı");
            }

            return ServiceResult<List<GetYoneticiTipiVm>>.Success(response.Content!);
        }
    }
}