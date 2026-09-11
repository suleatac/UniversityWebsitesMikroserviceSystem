using Microservice.Web.ViewModels.Video;
using Refit;

namespace Microservice.Web.Clients.VideoClients
{
    public interface IVideoClientServices
    {
        [Get("/api/v1/videos/{id}")]
        Task<ApiResponse<VideoDetailVm>> GetVideoByIdAsync(int id);
    }
}
