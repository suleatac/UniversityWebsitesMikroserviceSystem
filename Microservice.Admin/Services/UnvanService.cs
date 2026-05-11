using Microservice.Admin.Clients.UnvanClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Unvan;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class UnvanService : IUnvanService
    {
        private readonly IUnvanClientServices _unvanClient;
        private readonly ILogger<UnvanService> _logger;

        public UnvanService(IUnvanClientServices unvanClient, ILogger<UnvanService> logger)
        {
            _unvanClient = unvanClient ?? throw new ArgumentNullException(nameof(unvanClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // LIST
        public async Task<ServiceResult<List<GetUnvanVm>>> GetUnvansAsync()
        {
            _logger.LogInformation("API'den unvan listesi çekiliyor.");

            var response = await _unvanClient.GetUnvansAsync();

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

                return ServiceResult<List<GetUnvanVm>>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Unvanlar alınamadı"
                );
            }

            _logger.LogInformation("Unvan listesi başarıyla alındı. Count: {Count}", response.Content?.Count);
            return ServiceResult<List<GetUnvanVm>>.Success(response.Content!);
        }

        // GET BY ID
        public async Task<ServiceResult<UnvanVm>> GetUnvanByIdAsync(int id)
        {
            _logger.LogInformation("Unvan getiriliyor. Id: {Id}", id);

            var response = await _unvanClient.GetUnvanByIdAsync(id);

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

                return ServiceResult<UnvanVm>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Unvan alınamadı"
                );
            }

            return ServiceResult<UnvanVm>.Success(response.Content!);
        }

        // CREATE
        public async Task<ServiceResult<bool>> CreateUnvanAsync(UnvanVm dto)
        {
            _logger.LogInformation("Yeni unvan oluşturuluyor. Name: {Name}", dto.Ad);

            var response = await _unvanClient.CreateUnvanAsync(dto);

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
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Unvan oluşturulamadı"
                );
            }

            _logger.LogInformation("Unvan başarıyla oluşturuldu.");
            return ServiceResult<bool>.Success(true);
        }

        // UPDATE
        public async Task<ServiceResult<bool>> UpdateUnvanAsync(UnvanVm dto)
        {
            _logger.LogInformation("Unvan güncelleniyor. Id: {Id}", dto.Id);

            var response = await _unvanClient.UpdateUnvanAsync(dto.Id, dto);

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
                    problemDetails?.Detail ?? problemDetails?.Title ?? $"Unvan güncellenemedi. Id: {dto.Id}"
                );
            }

            _logger.LogInformation("Unvan güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<bool>.Success(true);
        }

        // DELETE
        public async Task<ServiceResult<bool>> DeleteUnvanAsync(int id)
        {
            _logger.LogWarning("Unvan siliniyor. Id: {Id}", id);

            var response = await _unvanClient.DeleteUnvanAsync(id);

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
                    problemDetails?.Detail ?? problemDetails?.Title ?? $"Unvan silinemedi. Id: {id}"
                );
            }

            _logger.LogInformation("Unvan silindi. Id: {Id}", id);
            return ServiceResult<bool>.Success(true);
        }
    }
}
