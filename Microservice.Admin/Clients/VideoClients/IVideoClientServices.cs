using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Video;
using Refit;

namespace Microservice.Admin.Clients.VideoClients
{
    public interface IVideoClientServices
    {
        [Get("/api/v1/videos")]
        Task<ApiResponse<List<GetVideoVm>>> GetVideosAsync(int siteId, int dilId);

        [Get("/api/v1/videos/{id}")]
        Task<ApiResponse<VideoDetailVm>> GetVideoByIdAsync(int id);

        [Post("/api/v1/videos")]
        Task<ApiResponse<object>> CreateVideoAsync([Body] CreateVideoVm dto);

        [Put("/api/v1/videos/{id}")]
        Task<ApiResponse<object>> UpdateVideoAsync(int id, [Body] VideoDetailVm dto);

        [Delete("/api/v1/videos/{id}")]
        Task<ApiResponse<object>> DeleteVideoAsync(int id);

        [Get("/api/v1/videos/paginated")]
        Task<ApiResponse<PaginatedResult<GetVideoVm>>> GetVideosPaginatedAsync(
          int siteId, int dilId,
          [AliasAs("page")] int page,
          [AliasAs("pageSize")] int pageSize,
          [AliasAs("search")] string? search,
          [AliasAs("orderBy")] string? orderBy,
          [AliasAs("orderDir")] string? orderDir);
    }
}