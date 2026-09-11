using Microservice.Web.ViewModels.Menu;
using Refit;

namespace Microservice.Web.Clients.MenuClients
{
    public interface IMenuClientServices
    {
        [Get("/api/v1/menus")]
        Task<ApiResponse<List<MenuGetVm>>> GetMenusAsync([Query] int siteId, [Query] int dilId, [Query] int? location = null);

        [Get("/api/v1/menus/seo/{siteId}/{dilId}/{seoUrl}")]
        Task<ApiResponse<MenuDetailVm>> GetMenuBySeoUrlAsync(int siteId, int dilId, string seoUrl);

        [Get("/api/v1/menus/{id}")]
        Task<ApiResponse<MenuDetailVm>> GetMenuByIdAsync(int id);
    }
}
