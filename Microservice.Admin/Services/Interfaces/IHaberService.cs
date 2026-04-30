using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Haber;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IHaberService
    {
        Task<ServiceResult<List<GetHaberVm>>> GetHabersAsync(int siteId, int dilId);
        Task<ServiceResult<HaberDetailVm>> GetHaberByIdAsync(int id);
        Task<ServiceResult<object>> CreateHaberAsync(CreateHaberVm dto);
        Task<ServiceResult<object>> UpdateHaberAsync(HaberDetailVm dto);
        Task<ServiceResult<object>> DeleteHaberAsync(int id);
    }
}
