using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Unvan;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IUnvanService
    {
        Task<ServiceResult<List<GetUnvanVm>>> GetUnvansAsync();
        Task<ServiceResult<UnvanVm>> GetUnvanByIdAsync(int id);
        Task<ServiceResult<bool>> CreateUnvanAsync(UnvanVm dto);
        Task<ServiceResult<bool>> UpdateUnvanAsync(UnvanVm dto);
        Task<ServiceResult<bool>> DeleteUnvanAsync(int id);
    }
}
