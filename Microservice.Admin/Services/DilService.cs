using Microservice.Admin.Clients.DilClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Dil;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class DilService: IDilService
    {
        private readonly IDilClientServices _dilRefitService;
        private readonly ILogger<DilService> _logger;

        public DilService(IDilClientServices dilRefitService, ILogger<DilService> logger)
        {
            _dilRefitService = dilRefitService ?? throw new ArgumentNullException(nameof(dilRefitService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        // LIST
        public async Task<ServiceResult<List<GetDilVm>>> GetDilsAsync()
        {
            _logger.LogInformation("API'den dil listesi çekiliyor.");

            var response = await _dilRefitService.GetDilsAsync();

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


                return ServiceResult<List<GetDilVm>>.Error(
                problemDetails?.Detail ?? problemDetails?.Title ?? "Diller alınamadı"
            );
            }

            _logger.LogInformation("Dil listesi başarıyla alındı. Count: {Count}", response.Content?.Count);
            return ServiceResult<List<GetDilVm>>.Success(response.Content!);
        }
    }
}
