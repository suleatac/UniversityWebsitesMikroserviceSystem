using Microservice.Web.ViewModels.Menu;
using Refit;

namespace Microservice.Web.Clients.MenuClients
{
    public interface IMenuClientServices
    {
        [Get("/api/v1/menus")]
        Task<ApiResponse<List<MenuGetVm>>> GetMenusAsync(int siteId, int dilId);
    }
}