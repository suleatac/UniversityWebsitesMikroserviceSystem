using Microservice.Admin.Clients.TumPersonelClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.TumPersonel;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class TumPersonelService: ITumPersonelService
    {
        private readonly ITumPersonelClientService _tumPersonelRefitService;
        private readonly ILogger<TumPersonelService> _logger;

        public TumPersonelService(ITumPersonelClientService tumPersonelRefitService, ILogger<TumPersonelService> logger)
        {
            _tumPersonelRefitService = tumPersonelRefitService ?? throw new ArgumentNullException(nameof(tumPersonelRefitService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        // LIST
        public async Task<ServiceResult<List<GetPersonelVm>>> GetTumPersonelsAsync()
        {
            _logger.LogInformation("API'den tüm personel listesi çekiliyor.");

            var response = await _tumPersonelRefitService.GetPersonelsAsync();

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


                return ServiceResult<List<GetPersonelVm>>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Tüm personeller alınamadı"
                );
            }

            _logger.LogInformation("Tüm personeller başarıyla alındı. Count: {Count}", response.Content?.Count);
            return ServiceResult<List<GetPersonelVm>>.Success(response.Content!);
        }

        // GET BY ID
        public async Task<ServiceResult<GetPersonelVm>> GetPersonelByIdAsync(int id)
        {
            _logger.LogInformation("Personel getiriliyor. Id: {Id}", id);

            var response = await _tumPersonelRefitService.GetPersonelByIdAsync(id);

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

                return ServiceResult<GetPersonelVm>
                    .Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Personeller alınamadı");

            }

            return ServiceResult<GetPersonelVm>.Success(response.Content!);
        }

    }
}
