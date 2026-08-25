using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Content;

namespace Microservice.Web.Services.Interfaces
{
    public interface IVideoService
    {
        Task<ServiceResult<ContentDetailVm>> GetVideoByIdAsync(int id);
    }
}
