using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
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
        Task<ServiceResult<PaginatedResult<GetHaberVm>>> GetHabersPaginatedAsync
                  (
                     int siteId,
                     int dilId,
                     int page,
                     int pageSize,
                     string? search,
                     string? orderBy,
                     string? orderDir
                  );
    }
}
