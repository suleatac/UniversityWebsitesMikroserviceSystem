using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Content;

namespace Microservice.Web.Services.Interfaces
{
    public interface IBilgiService
    {
        Task<ServiceResult<ContentDetailVm>> GetBilgiByIdAsync(int id);
    }
}
