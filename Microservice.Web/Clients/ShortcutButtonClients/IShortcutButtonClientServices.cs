using Microservice.Web.ViewModels.ShortcutButton;
using Refit;

namespace Microservice.Web.Clients.ShortcutButtonClients
{
    public interface IShortcutButtonClientServices
    {
        [Get("/api/v1/shortcutButtons")]
        Task<ApiResponse<List<GetShortcutButtonVm>>> GetShortcutButtonsAsync([Query] int siteId, [Query] int dilId);

        [Get("/api/v1/shortcutButtons/{id}")]
        Task<ApiResponse<ShortcutButtonVm>> GetShortcutButtonByIdAsync(int id);

       
    }
}
