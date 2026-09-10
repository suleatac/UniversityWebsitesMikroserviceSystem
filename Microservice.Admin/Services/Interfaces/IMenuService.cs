using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Menu;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IMenuService
    {
        Task<ServiceResult<List<GetMenuVm>>> GetMenusAsync(int siteId, int dilId, int? location = null);
        Task<ServiceResult<MenuDetailVm>> GetMenuByIdAsync(int id);
        Task<ServiceResult<object>> CreateMenuAsync(MenuDetailVm dto);
        Task<ServiceResult<object>> UpdateMenuAsync(MenuDetailVm dto);
        Task<ServiceResult<bool>> DeleteMenuAsync(int id);
    }
}