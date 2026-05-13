using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Popup;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IPopupService
    {
        Task<ServiceResult<PopupDetailVm>> GetPopupBySiteIdAsync(int siteId);
        Task<ServiceResult<PopupDetailVm>> GetPopupByIdAsync(int id);
        Task<ServiceResult<object>> CreatePopupAsync(CreatePopupVm dto);
        Task<ServiceResult<object>> UpdatePopupAsync(PopupDetailVm dto);
        Task<ServiceResult<object>> DeletePopupAsync(int id);
    }
}