using Microservice.Admin.ViewModels.YoneticiSite;
using Refit;

namespace Microservice.Admin.Clients.YoneticiSiteClients
{
    public interface IYoneticiSiteClientServices
    {
        [Get("/api/v1/yonetici-siteler")]
        Task<ApiResponse<List<GetYoneticiSiteVm>>> GetYoneticiSitesAsync(int siteId);

        [Get("/api/v1/yonetici-siteler/{id}")]
        Task<ApiResponse<YoneticiSiteDetailVm>> GetYoneticiSiteByIdAsync(int id);

        [Post("/api/v1/yonetici-siteler")]
        Task<ApiResponse<object>> CreateYoneticiSiteAsync([Body] CreateYoneticiSiteVm dto);

        [Put("/api/v1/yonetici-siteler/{id}")]
        Task<ApiResponse<object>> UpdateYoneticiSiteAsync(int id, [Body] YoneticiSiteDetailVm dto);

        [Delete("/api/v1/yonetici-siteler/{id}")]
        Task<ApiResponse<object>> DeleteYoneticiSiteAsync(int id);
    }
}