using Microservice.Web.Clients.ShortcutButtonClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.ShortcutButton;
using System.Text.Json;

namespace Microservice.Web.Services
{
    public class ShortcutButtonService: IShortcutButtonService
    {
        private readonly IShortcutButtonClientServices _shortcutButtonClient;
        private readonly ILogger<ShortcutButtonService> _logger;

        public ShortcutButtonService(IShortcutButtonClientServices shortcutButtonClient, ILogger<ShortcutButtonService> logger)
        {
            _shortcutButtonClient = shortcutButtonClient ?? throw new ArgumentNullException(nameof(shortcutButtonClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

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

    }
}
