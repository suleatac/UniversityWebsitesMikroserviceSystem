using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Content;

namespace Microservice.Web.Services.Interfaces
{
    public interface IBannerService
    {
        Task<ServiceResult<ContentDetailVm>> GetBannerByIdAsync(int id);
        Task<ServiceResult<List<ContentDetailVm>>> GetBannersAsync(int siteId, int dilId);
    }
}
