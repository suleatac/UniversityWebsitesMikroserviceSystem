using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.ShortcutButton;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IShortcutButtonService
    {
        Task<ServiceResult<List<GetShortcutButtonVm>>> GetShortcutButtonsAsync(int siteId, int dilId);
        Task<ServiceResult<ShortcutButtonVm>> GetShortcutButtonByIdAsync(int id);
        Task<ServiceResult<bool>> CreateShortcutButtonAsync(ShortcutButtonVm dto);
        Task<ServiceResult<bool>> UpdateShortcutButtonAsync(ShortcutButtonVm dto);
        Task<ServiceResult<bool>> DeleteShortcutButtonAsync(int id);
    }
}
