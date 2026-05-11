using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Popup;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IPopupService
    {
        Task<ServiceResult<List<GetPopupVm>>> GetPopupsAsync(int siteId, int dilId);
        Task<ServiceResult<PopupDetailVm>> GetPopupByIdAsync(int id);
        Task<ServiceResult<object>> CreatePopupAsync(CreatePopupVm dto);
        Task<ServiceResult<object>> UpdatePopupAsync(PopupDetailVm dto);
        Task<ServiceResult<object>> DeletePopupAsync(int id);
        Task<ServiceResult<PaginatedResult<GetPopupVm>>> GetPopupsPaginatedAsync(int siteId, int dilId, int page, int pageSize, string? search, string? orderBy, string? orderDir);
    }
}