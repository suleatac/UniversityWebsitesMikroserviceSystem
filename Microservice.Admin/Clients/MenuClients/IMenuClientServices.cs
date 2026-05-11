using Microservice.Admin.ViewModels.Menu;
using Refit;

namespace Microservice.Admin.Clients.MenuClients
{
    public interface IMenuClientServices
    {
        [Get("/api/v1/menus")]
        Task<ApiResponse<List<GetMenuVm>>> GetMenusAsync([Query] int siteId, [Query] int dilId);

        [Get("/api/v1/menus/{id}")]
        Task<ApiResponse<MenuVm>> GetMenuByIdAsync(int id);

        [Post("/api/v1/menus")]
        Task<ApiResponse<object>> CreateMenuAsync([Body] MenuVm dto);

        [Put("/api/v1/menus/{id}")]
        Task<ApiResponse<object>> UpdateMenuAsync(int id, [Body] MenuVm dto);

        [Delete("/api/v1/menus/{id}")]
        Task<ApiResponse<object>> DeleteMenuAsync(int id);
    }
}