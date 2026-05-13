using Microservice.Admin.Clients.PopupClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Popup;

namespace Microservice.Admin.Services
{
    public class PopupService : IPopupService
    {
        private readonly IPopupClientServices _popupClient;
        private readonly ILogger<PopupService> _logger;

        public PopupService(IPopupClientServices popupClient, ILogger<PopupService> logger)
        {
            _popupClient = popupClient;
            _logger = logger;
        }

        public async Task<ServiceResult<PopupDetailVm>> GetPopupBySiteIdAsync(int siteId)
        {
            _logger.LogInformation("Popup getiriliyor. SiteId: {SiteId}", siteId);
            var response = await _popupClient.GetPopupBySiteIdAsync(siteId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error?.Content;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails, problemDetails);
                return ServiceResult<PopupDetailVm>.Error(problemDetails ?? "Popup bulunamadı");
            }

            return ServiceResult<PopupDetailVm>.Success(response.Content!);
        }

        public async Task<ServiceResult<PopupDetailVm>> GetPopupByIdAsync(int id)
        {
            _logger.LogInformation("Popup getiriliyor. Id: {Id}", id);
            var response = await _popupClient.GetPopupByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error?.Content;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails, problemDetails);
                return ServiceResult<PopupDetailVm>.Error(problemDetails ?? "Popup bulunamadı");
            }

            return ServiceResult<PopupDetailVm>.Success(response.Content!);
        }

        public async Task<ServiceResult<object>> CreatePopupAsync(CreatePopupVm dto)
        {
            _logger.LogInformation("Yeni popup oluşturuluyor. Başlık: {Title}", dto.Baslik);
            var response = await _popupClient.CreatePopupAsync(dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error?.Content;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails, problemDetails);
                return ServiceResult<object>.Error(problemDetails ?? "Popup oluşturulamadı");
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
                var problemDetails = response.Error?.Content;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails, problemDetails);
                return ServiceResult<object>.Error(problemDetails ?? $"Popup güncellenemedi. Id: {dto.Id}");
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
                var problemDetails = response.Error?.Content;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails, problemDetails);
                return ServiceResult<object>.Error(problemDetails ?? "Popup silinemedi");
            }

            _logger.LogInformation("Popup silindi. Id: {Id}", id);
            return ServiceResult<object>.Success(true);
        }
    }
}