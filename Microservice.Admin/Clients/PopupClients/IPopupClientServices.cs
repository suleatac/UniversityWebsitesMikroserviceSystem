using Microservice.Admin.ViewModels.Popup;
using Refit;

namespace Microservice.Admin.Clients.PopupClients
{
    public interface IPopupClientServices
    {
        [Get("/api/v1/popups")]
        Task<ApiResponse<PopupDetailVm>> GetPopupBySiteIdAsync(int siteId);

        [Get("/api/v1/popups/{id}")]
        Task<ApiResponse<PopupDetailVm>> GetPopupByIdAsync(int id);

        [Post("/api/v1/popups")]
        Task<ApiResponse<object>> CreatePopupAsync([Body] CreatePopupVm dto);

        [Put("/api/v1/popups/{id}")]
        Task<ApiResponse<object>> UpdatePopupAsync(int id, [Body] PopupDetailVm dto);

        [Delete("/api/v1/popups/{id}")]
        Task<ApiResponse<object>> DeletePopupAsync(int id);
    }
}