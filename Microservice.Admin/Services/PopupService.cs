using Microservice.Admin.Clients.PopupClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Popup;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class PopupService : IPopupService
    {
        private readonly IPopupClientServices _popupClient;
        private readonly ILogger<PopupService> _logger;

        public PopupService(IPopupClientServices popupClient, ILogger<PopupService> logger)
        {
            _popupClient = popupClient ?? throw new ArgumentNullException(nameof(popupClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<List<GetPopupVm>>> GetPopupsAsync(int siteId, int dilId)
        {
            _logger.LogInformation("Popup listesi çekiliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);
            var response = await _popupClient.GetPopupsAsync(siteId, dilId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<List<GetPopupVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Popup listesi alınamadı");
            }

            return ServiceResult<List<GetPopupVm>>.Success(response.Content!);
        }

        public async Task<ServiceResult<PopupDetailVm>> GetPopupByIdAsync(int id)
        {
            _logger.LogInformation("Popup getiriliyor. Id: {Id}", id);
            var response = await _popupClient.GetPopupByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<PopupDetailVm>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Popup bulunamadı");
            }

            return ServiceResult<PopupDetailVm>.Success(response.Content!);
        }

        public async Task<ServiceResult<object>> CreatePopupAsync(CreatePopupVm dto)
        {
            _logger.LogInformation("Yeni popup oluşturuluyor. Başlık: {Title}", dto.Baslik);
            var response = await _popupClient.CreatePopupAsync(dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Popup oluşturulamadı");
            }

            _logger.LogInformation("Popup oluşturuldu.");
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> UpdatePopupAsync(PopupDetailVm dto)
        {
            _logger.LogInformation("Popup güncelleniyor. Id: {Id}", dto.Id);
            var response = await _popupClient.UpdatePopupAsync(dto.Id, dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? $"Popup güncellenemedi. Id: {dto.Id}");
            }

            _logger.LogInformation("Popup güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> DeletePopupAsync(int id)
        {
            _logger.LogWarning("Popup silme isteği alındı. Id: {Id}", id);
            var response = await _popupClient.DeletePopupAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Popup silinemedi");
            }

            _logger.LogInformation("Popup silindi. Id: {Id}", id);
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<PaginatedResult<GetPopupVm>>> GetPopupsPaginatedAsync(int siteId, int dilId, int page, int pageSize, string? search, string? orderBy, string? orderDir)
        {
            _logger.LogInformation("Paginated popup listesi çekiliyor. Page: {Page}, PageSize: {PageSize}", page, pageSize);
            var response = await _popupClient.GetPopupsPaginatedAsync(siteId, dilId, page, pageSize, search, orderBy, orderDir);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}", response.StatusCode, problemDetails?.Title);
                return ServiceResult<PaginatedResult<GetPopupVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Paginated popup listesi alınamadı");
            }

            return ServiceResult<PaginatedResult<GetPopupVm>>.Success(response.Content!);
        }
    }
}