using Microservice.Admin.Clients.ShortcutButtonClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.ShortcutButton;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class ShortcutButtonService : IShortcutButtonService
    {
        private readonly IShortcutButtonClientServices _shortcutButtonClient;
        private readonly ILogger<ShortcutButtonService> _logger;

        public ShortcutButtonService(IShortcutButtonClientServices shortcutButtonClient, ILogger<ShortcutButtonService> logger)
        {
            _shortcutButtonClient = shortcutButtonClient ?? throw new ArgumentNullException(nameof(shortcutButtonClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // LIST
        public async Task<ServiceResult<List<GetShortcutButtonVm>>> GetShortcutButtonsAsync(int siteId, int dilId)
        {
            _logger.LogInformation("API'den shortcut button listesi çekiliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);

            var response = await _shortcutButtonClient.GetShortcutButtonsAsync(siteId, dilId);

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

                return ServiceResult<List<GetShortcutButtonVm>>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Kısayol butonları alınamadı"
                );
            }

            _logger.LogInformation("Shortcut button listesi başarıyla alındı. Count: {Count}", response.Content?.Count);
            return ServiceResult<List<GetShortcutButtonVm>>.Success(response.Content!);
        }

        // GET BY ID
        public async Task<ServiceResult<ShortcutButtonVm>> GetShortcutButtonByIdAsync(int id)
        {
            _logger.LogInformation("Shortcut button getiriliyor. Id: {Id}", id);

            var response = await _shortcutButtonClient.GetShortcutButtonByIdAsync(id);

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

                return ServiceResult<ShortcutButtonVm>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Kısayol butonu alınamadı"
                );
            }

            return ServiceResult<ShortcutButtonVm>.Success(response.Content!);
        }

        // CREATE
        public async Task<ServiceResult<bool>> CreateShortcutButtonAsync(ShortcutButtonVm dto)
        {
            _logger.LogInformation("Yeni shortcut button oluşturuluyor. Ad: {Ad}", dto.Ad);

            var response = await _shortcutButtonClient.CreateShortcutButtonAsync(dto);

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
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Kısayol butonu oluşturulamadı"
                );
            }

            _logger.LogInformation("Shortcut button başarıyla oluşturuldu.");
            return ServiceResult<bool>.Success(true);
        }

        // UPDATE
        public async Task<ServiceResult<bool>> UpdateShortcutButtonAsync(ShortcutButtonVm dto)
        {
            _logger.LogInformation("Shortcut button güncelleniyor. Id: {Id}", dto.Id);

            var response = await _shortcutButtonClient.UpdateShortcutButtonAsync(dto.Id, dto);

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
                    problemDetails?.Detail ?? problemDetails?.Title ?? $"Kısayol butonu güncellenemedi. Id: {dto.Id}"
                );
            }

            _logger.LogInformation("Shortcut button güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<bool>.Success(true);
        }

        // DELETE
        public async Task<ServiceResult<bool>> DeleteShortcutButtonAsync(int id)
        {
            _logger.LogWarning("Shortcut button siliniyor. Id: {Id}", id);

            var response = await _shortcutButtonClient.DeleteShortcutButtonAsync(id);

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
                    problemDetails?.Detail ?? problemDetails?.Title ?? $"Kısayol butonu silinemedi. Id: {id}"
                );
            }

            _logger.LogInformation("Shortcut button silindi. Id: {Id}", id);
            return ServiceResult<bool>.Success(true);
        }
    }
}
