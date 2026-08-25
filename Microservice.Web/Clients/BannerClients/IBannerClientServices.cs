using Microservice.Web.ViewModels.Content;
using Refit;

namespace Microservice.Web.Clients.BannerClients
{
    public interface IBannerClientServices
    {
        [Get("/api/v1/banners/{id}")]
        Task<ApiResponse<ContentDetailVm>> GetBannerByIdAsync(int id);
    }
}
