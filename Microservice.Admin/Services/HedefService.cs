using Microservice.Admin.Clients.HedefClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Hedef;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class HedefService:IHedefService
    {
        private readonly IHedefClientServices _hedefRefitService;
        private readonly ILogger<HedefService> _logger;

        public HedefService(IHedefClientServices hedefRefitService, ILogger<HedefService> logger)
        {
            _hedefRefitService = hedefRefitService ?? throw new ArgumentNullException(nameof(hedefRefitService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        // LIST
        public async Task<ServiceResult<List<GetHedefVm>>> GetHedefsAsync()
        {
            _logger.LogInformation("API'den hedef listesi çekiliyor.");

            var response = await _hedefRefitService.GetHedefsAsync();

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


                return ServiceResult<List<GetHedefVm>>.Error(
                problemDetails?.Detail ?? problemDetails?.Title ?? "Hedefler alınamadı"
            );
            }

            _logger.LogInformation("Hedef listesi başarıyla alındı. Count: {Count}", response.Content?.Count);
            return ServiceResult<List<GetHedefVm>>.Success(response.Content!);
        }
    }
}
