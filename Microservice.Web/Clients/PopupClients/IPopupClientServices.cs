using Microservice.Web.ViewModels.Popup;
using Refit;

namespace Microservice.Web.Clients.PopupClients
{
    public interface IPopupClientServices
    {
        // Site API popup'u site bazinda tek kayit olarak dondurur; kayit yoksa 404 doner.
        [Get("/api/v1/popups")]
        Task<ApiResponse<GetPopupVm>> GetPopupBySiteIdAsync(int siteId);
    }
}
