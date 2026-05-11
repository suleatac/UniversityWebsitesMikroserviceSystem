using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Popup;
using Refit;

namespace Microservice.Admin.Clients.PopupClients
{
    public interface IPopupClientServices
    {
        [Get("/api/v1/popups")]
        Task<ApiResponse<List<GetPopupVm>>> GetPopupsAsync(int siteId, int dilId);

        [Get("/api/v1/popups/{id}")]
        Task<ApiResponse<PopupDetailVm>> GetPopupByIdAsync(int id);

        [Post("/api/v1/popups")]
        Task<ApiResponse<object>> CreatePopupAsync([Body] CreatePopupVm dto);

        [Put("/api/v1/popups/{id}")]
        Task<ApiResponse<object>> UpdatePopupAsync(int id, [Body] PopupDetailVm dto);

        [Delete("/api/v1/popups/{id}")]
        Task<ApiResponse<object>> DeletePopupAsync(int id);

        [Get("/api/v1/popups/paginated")]
        Task<ApiResponse<PaginatedResult<GetPopupVm>>> GetPopupsPaginatedAsync(
            int siteId, int dilId,
            [AliasAs("page")] int page,
            [AliasAs("pageSize")] int pageSize,
            [AliasAs("search")] string? search,
            [AliasAs("orderBy")] string? orderBy,
            [AliasAs("orderDir")] string? orderDir);
    }
}