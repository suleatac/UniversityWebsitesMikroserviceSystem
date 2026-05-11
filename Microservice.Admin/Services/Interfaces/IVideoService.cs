using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Video;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IVideoService
    {
        Task<ServiceResult<List<GetVideoVm>>> GetVideosAsync(int siteId, int dilId);
        Task<ServiceResult<VideoDetailVm>> GetVideoByIdAsync(int id);
        Task<ServiceResult<object>> CreateVideoAsync(CreateVideoVm dto);
        Task<ServiceResult<object>> UpdateVideoAsync(VideoDetailVm dto);
        Task<ServiceResult<object>> DeleteVideoAsync(int id);
        Task<ServiceResult<PaginatedResult<GetVideoVm>>> GetVideosPaginatedAsync(
            int siteId, int dilId, int page, int pageSize, string? search, string? orderBy, string? orderDir);
    }
}