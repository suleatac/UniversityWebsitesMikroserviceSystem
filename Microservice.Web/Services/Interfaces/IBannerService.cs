using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Content;

namespace Microservice.Web.Services.Interfaces
{
    public interface IBannerService
    {
        Task<ServiceResult<ContentDetailVm>> GetBannerByIdAsync(int id);
    }
}
