using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.GaleriResim;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IGaleriResimService
    {
        Task<ServiceResult<List<GetGaleriResimVm>>> GetGaleriResimlerAsync(int siteId, int dilId);
        Task<ServiceResult<GaleriResimDetailVm>> GetGaleriResimByIdAsync(int id);
        Task<ServiceResult<object>> CreateGaleriResimAsync(CreateGaleriResimVm dto);
        Task<ServiceResult<object>> UpdateGaleriResimAsync(GaleriResimDetailVm dto);
        Task<ServiceResult<object>> DeleteGaleriResimAsync(int id);
        Task<ServiceResult<PaginatedResult<GetGaleriResimVm>>> GetGaleriResimlerPaginatedAsync(
            int siteId, int dilId, int page, int pageSize, string? search, string? orderBy, string? orderDir);
    }
}
