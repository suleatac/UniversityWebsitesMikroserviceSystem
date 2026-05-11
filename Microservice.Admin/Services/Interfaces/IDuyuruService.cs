using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Duyuru;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IDuyuruService
    {
        Task<ServiceResult<List<GetDuyuruVm>>> GetDuyurularAsync(int siteId, int dilId);
        Task<ServiceResult<DuyuruDetailVm>> GetDuyuruByIdAsync(int id);
        Task<ServiceResult<object>> CreateDuyuruAsync(CreateDuyuruVm dto);
        Task<ServiceResult<object>> UpdateDuyuruAsync(DuyuruDetailVm dto);
        Task<ServiceResult<object>> DeleteDuyuruAsync(int id);
        Task<ServiceResult<PaginatedResult<GetDuyuruVm>>> GetDuyurularPaginatedAsync(
            int siteId, int dilId, int page, int pageSize, string? search, string? orderBy, string? orderDir);
    }
}