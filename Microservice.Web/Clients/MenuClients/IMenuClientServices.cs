using Microservice.Web.ViewModels.Menu;
using Refit;

namespace Microservice.Web.Clients.MenuClients
{
    public interface IMenuClientServices
    {
        [Get("/api/v1/menus")]
        Task<ApiResponse<List<MenuGetVm>>> GetMenusAsync([Query] int siteId, [Query] int dilId, [Query] int? location = null);
    }
}
