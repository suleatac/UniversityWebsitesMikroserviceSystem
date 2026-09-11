using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Banner;

namespace Microservice.Web.Services.Interfaces
{
    public interface IBannerService
    {
        Task<ServiceResult<BannerDetailVm>> GetBannerByIdAsync(int id);
        Task<ServiceResult<List<GetBannerVm>>> GetBannersAsync(int siteId, int dilId);
    }
}
