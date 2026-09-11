using Microservice.Web.ViewModels.Banner;
using Refit;

namespace Microservice.Web.Clients.BannerClients
{
    public interface IBannerClientServices
    {
        [Get("/api/v1/banners/{id}")]
        Task<ApiResponse<BannerDetailVm>> GetBannerByIdAsync(int id);

        [Get("/api/v1/banners")]
        Task<ApiResponse<List<GetBannerVm>>> GetBannersAsync(int siteId, int dilId);
    }
}
