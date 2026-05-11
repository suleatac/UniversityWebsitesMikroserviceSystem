using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Banner;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IBannerService
    {
        Task<ServiceResult<List<GetBannerVm>>> GetBannersAsync(int siteId, int dilId);
        Task<ServiceResult<BannerDetailVm>> GetBannerByIdAsync(int id);
        Task<ServiceResult<object>> CreateBannerAsync(CreateBannerVm dto);
        Task<ServiceResult<object>> UpdateBannerAsync(BannerDetailVm dto);
        Task<ServiceResult<object>> DeleteBannerAsync(int id);
        Task<ServiceResult<PaginatedResult<GetBannerVm>>> GetBannersPaginatedAsync(
            int siteId, int dilId, int page, int pageSize, string? search, string? orderBy, string? orderDir);
    }
}