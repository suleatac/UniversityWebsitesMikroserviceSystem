using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Video;

namespace Microservice.Web.Services.Interfaces
{
    public interface IVideoService
    {
        Task<ServiceResult<VideoDetailVm>> GetVideoByIdAsync(int id);

        // Ana sayfa "Tanitimi Videolari" bolumu icin site+ dile gore video listesi.
        Task<ServiceResult<List<GetVideoVm>>> GetVideolarAsync(int siteId, int dilId);
    }
}
