using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Menu;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IMenuService
    {
        Task<ServiceResult<List<GetMenuVm>>> GetMenusAsync(int siteId, int dilId);
        Task<ServiceResult<MenuVm>> GetMenuByIdAsync(int id);
        Task<ServiceResult<bool>> CreateMenuAsync(MenuVm dto);
        Task<ServiceResult<bool>> UpdateMenuAsync(MenuVm dto);
        Task<ServiceResult<bool>> DeleteMenuAsync(int id);
    }
}