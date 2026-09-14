using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Bilgi;

namespace Microservice.Web.Services.Interfaces
{
    public interface IBilgiService
    {
        Task<ServiceResult<List<GetBilgiVm>>> GetBilgisAsync(int siteId, int dilId);
        Task<ServiceResult<BilgiDetailVm>> GetBilgiByIdAsync(int id);
    }
}
