using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.ShortcutButton;

namespace Microservice.Web.Services.Interfaces
{
    public interface IShortcutButtonService
    {
        Task<ServiceResult<List<GetShortcutButtonVm>>> GetShortcutButtonsAsync(int siteId, int dilId);
        Task<ServiceResult<ShortcutButtonVm>> GetShortcutButtonByIdAsync(int id);

    }
}
