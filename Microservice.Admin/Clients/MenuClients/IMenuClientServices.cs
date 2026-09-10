using Microservice.Admin.ViewModels.Menu;
using Refit;

namespace Microservice.Admin.Clients.MenuClients
{
    public interface IMenuClientServices
    {
        [Get("/api/v1/menus")]
        Task<ApiResponse<List<GetMenuVm>>> GetMenusAsync([Query] int siteId, [Query] int dilId, [Query] int? location = null);

        [Get("/api/v1/menus/{id}")]
        Task<ApiResponse<MenuDetailVm>> GetMenuByIdAsync(int id);

        [Post("/api/v1/menus")]
        Task<ApiResponse<object>> CreateMenuAsync([Body] MenuDetailVm dto);

        [Put("/api/v1/menus/{id}")]
        Task<ApiResponse<object>> UpdateMenuAsync(int id, [Body] MenuDetailVm dto);

        [Delete("/api/v1/menus/{id}")]
        Task<ApiResponse<object>> DeleteMenuAsync(int id);
    }
}