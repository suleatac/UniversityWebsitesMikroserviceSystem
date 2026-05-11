using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Bilgi;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IBilgiService
    {
        Task<ServiceResult<List<GetBilgiVm>>> GetBilgilerAsync(int siteId, int dilId);
        Task<ServiceResult<BilgiDetailVm>> GetBilgiByIdAsync(int id);
        Task<ServiceResult<object>> CreateBilgiAsync(CreateBilgiVm dto);
        Task<ServiceResult<object>> UpdateBilgiAsync(BilgiDetailVm dto);
        Task<ServiceResult<object>> DeleteBilgiAsync(int id);
        Task<ServiceResult<PaginatedResult<GetBilgiVm>>> GetBilgilerPaginatedAsync(
            int siteId, int dilId, int page, int pageSize, string? search, string? orderBy, string? orderDir);
    }
}