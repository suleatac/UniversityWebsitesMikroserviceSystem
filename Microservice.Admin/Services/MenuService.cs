using Microservice.Admin.Clients.MenuClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Menu;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuClientServices _menuClient;
        private readonly ILogger<MenuService> _logger;

        public MenuService(IMenuClientServices menuClient, ILogger<MenuService> logger)
        {
            _menuClient = menuClient ?? throw new ArgumentNullException(nameof(menuClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // LIST
        public async Task<ServiceResult<List<GetMenuVm>>> GetMenusAsync(int siteId, int dilId)
        {
            _logger.LogInformation("API'den menu listesi çekiliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);

            var response = await _menuClient.GetMenusAsync(siteId, dilId);

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

                return ServiceResult<List<GetMenuVm>>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Menüler alınamadı"
                );
            }

            _logger.LogInformation("Menu listesi başarıyla alındı. Count: {Count}", response.Content?.Count);
            return ServiceResult<List<GetMenuVm>>.Success(response.Content!);
        }

        // GET BY ID
        public async Task<ServiceResult<MenuVm>> GetMenuByIdAsync(int id)
        {
            _logger.LogInformation("Menu getiriliyor. Id: {Id}", id);

            var response = await _menuClient.GetMenuByIdAsync(id);

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

                return ServiceResult<MenuVm>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Menu alınamadı"
                );
            }

            return ServiceResult<MenuVm>.Success(response.Content!);
        }

        // CREATE
        public async Task<ServiceResult<bool>> CreateMenuAsync(MenuVm dto)
        {
            _logger.LogInformation("Yeni menu oluşturuluyor. Name: {Name}", dto.Ad);

            var response = await _menuClient.CreateMenuAsync(dto);

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
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Menu oluşturulamadı"
                );
            }

            _logger.LogInformation("Menu başarıyla oluşturuldu.");
            return ServiceResult<bool>.Success(true);
        }

        // UPDATE
        public async Task<ServiceResult<bool>> UpdateMenuAsync(MenuVm dto)
        {
            _logger.LogInformation("Menu güncelleniyor. Id: {Id}", dto.Id);

            var response = await _menuClient.UpdateMenuAsync(dto.Id, dto);

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
                    problemDetails?.Detail ?? problemDetails?.Title ?? $"Menu güncellenemedi. Id: {dto.Id}"
                );
            }

            _logger.LogInformation("Menu güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<bool>.Success(true);
        }

        // DELETE
        public async Task<ServiceResult<bool>> DeleteMenuAsync(int id)
        {
            _logger.LogWarning("Menu siliniyor. Id: {Id}", id);

            var response = await _menuClient.DeleteMenuAsync(id);

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
                    problemDetails?.Detail ?? problemDetails?.Title ?? $"Menu silinemedi. Id: {id}"
                );
            }

            _logger.LogInformation("Menu silindi. Id: {Id}", id);
            return ServiceResult<bool>.Success(true);
        }
    }
}