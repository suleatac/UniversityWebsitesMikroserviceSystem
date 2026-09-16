using Microservice.Web.ViewModels.Video;
using Refit;

namespace Microservice.Web.Clients.VideoClients
{
    public interface IVideoClientServices
    {
        [Get("/api/v1/videos/{id}")]
        Task<ApiResponse<VideoDetailVm>> GetVideoByIdAsync(int id);

        [Get("/api/v1/videos/seo/{siteId}/{dilId}/{seoUrl}")]
        Task<ApiResponse<VideoDetailVm>> GetVideoBySeoUrlAsync(int siteId, int dilId, string seoUrl);
        [Get("/api/v1/videos")]
        Task<ApiResponse<List<GetVideoVm>>> GetVideolarAsync(int siteId, int dilId);
    }
}
