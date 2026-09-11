using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Video;

namespace Microservice.Web.Services.Interfaces
{
    public interface IVideoService
    {
        Task<ServiceResult<VideoDetailVm>> GetVideoByIdAsync(int id);
    }
}
