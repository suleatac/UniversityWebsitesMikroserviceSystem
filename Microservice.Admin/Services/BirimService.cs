using Microservice.Admin.Clients.BirimClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Birim;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class BirimService : IBirimService
    {
        private readonly IBirimClientServices _birimClient;
        private readonly ILogger<BirimService> _logger;

        public BirimService(IBirimClientServices birimClient, ILogger<BirimService> logger)
        {
            _birimClient = birimClient ?? throw new ArgumentNullException(nameof(birimClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // LIST
        public async Task<ServiceResult<List<GetBirimVm>>> GetBirimsAsync()
        {
            _logger.LogInformation("API'den birim listesi çekiliyor.");

            var response = await _birimClient.GetBirimsAsync();

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

                return ServiceResult<List<GetBirimVm>>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Birimler alınamadı"
                );
            }

            _logger.LogInformation("Birim listesi başarıyla alındı. Count: {Count}", response.Content?.Count);
            return ServiceResult<List<GetBirimVm>>.Success(response.Content!);
        }

        // GET BY ID
        public async Task<ServiceResult<GetBirimDetailVm>> GetBirimByIdAsync(int id)
        {
            _logger.LogInformation("Birim getiriliyor. Id: {Id}", id);

            var response = await _birimClient.GetBirimByIdAsync(id);

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

                return ServiceResult<GetBirimDetailVm>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Birim alınamadı"
                );
            }

            return ServiceResult<GetBirimDetailVm>.Success(response.Content!);
        }

        // CREATE
        public async Task<ServiceResult<bool>> CreateBirimAsync(CreateBirimVm dto)
        {
            _logger.LogInformation("Yeni birim oluşturuluyor. Name: {Name}", dto.Ad);

            var response = await _birimClient.CreateBirimAsync(dto);

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

                return ServiceResult<bool>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Birim oluşturulamadı"
                );
            }

            _logger.LogInformation("Birim başarıyla oluşturuldu.");
            return ServiceResult<bool>.Success(true);
        }

        // UPDATE
        public async Task<ServiceResult<bool>> UpdateBirimAsync(UpdateBirimVm dto)
        {
            _logger.LogInformation("Birim güncelleniyor. Id: {Id}", dto.Id);

            var response = await _birimClient.UpdateBirimAsync(dto.Id, dto);

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

                return ServiceResult<bool>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? $"Birim güncellenemedi. Id: {dto.Id}"
                );
            }

            _logger.LogInformation("Birim güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<bool>.Success(true);
        }

        // DELETE
        public async Task<ServiceResult<bool>> DeleteBirimAsync(int id)
        {
            _logger.LogWarning("Birim siliniyor. Id: {Id}", id);

            var response = await _birimClient.DeleteBirimAsync(id);

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

                return ServiceResult<bool>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? $"Birim silinemedi. Id: {id}"
                );
            }

            _logger.LogInformation("Birim silindi. Id: {Id}", id);
            return ServiceResult<bool>.Success(true);
        }
    }
}
