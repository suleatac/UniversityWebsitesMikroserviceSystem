using Microservice.Web.ViewModels.Content;
using Refit;

namespace Microservice.Web.Clients.VideoClients
{
    public interface IVideoClientServices
    {
        [Get("/api/v1/videos/{id}")]
        Task<ApiResponse<ContentDetailVm>> GetVideoByIdAsync(int id);
    }
}
