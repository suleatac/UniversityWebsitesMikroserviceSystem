using Microservice.Admin.ViewModels.ShortcutButton;
using Refit;

namespace Microservice.Admin.Clients.ShortcutButtonClients
{
    public interface IShortcutButtonClientServices
    {
        [Get("/api/v1/shortcutButtons")]
        Task<ApiResponse<List<GetShortcutButtonVm>>> GetShortcutButtonsAsync([Query] int siteId, [Query] int dilId);

        [Get("/api/v1/shortcutButtons/{id}")]
        Task<ApiResponse<ShortcutButtonVm>> GetShortcutButtonByIdAsync(int id);

        [Post("/api/v1/shortcutButtons")]
        Task<ApiResponse<object>> CreateShortcutButtonAsync([Body] ShortcutButtonVm dto);

        [Put("/api/v1/shortcutButtons/{id}")]
        Task<ApiResponse<object>> UpdateShortcutButtonAsync(int id, [Body] ShortcutButtonVm dto);

        [Delete("/api/v1/shortcutButtons/{id}")]
        Task<ApiResponse<object>> DeleteShortcutButtonAsync(int id);
    }
}
