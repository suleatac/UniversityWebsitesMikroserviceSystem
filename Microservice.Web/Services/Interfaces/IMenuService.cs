using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Menu;

namespace Microservice.Web.Services.Interfaces
{
    public interface IMenuService
    {
        Task<ServiceResult<List<MenuGetVm>>> GetMenusAsync(int siteId, int dilId);
    }
}