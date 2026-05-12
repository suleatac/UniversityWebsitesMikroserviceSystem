using Microservice.Admin.ViewModels.YoneticiSite;
using Refit;

namespace Microservice.Admin.Clients.YoneticiSiteClients
{
    public interface IYoneticiSiteClientServices
    {
        [Get("/api/v1/yonetici-siteler")]
        Task<ApiResponse<List<YoneticiSiteDetailVm>>> GetYoneticiSitesAsync();

        [Get("/api/v1/yonetici-siteler/{id}")]
        Task<ApiResponse<YoneticiSiteDetailVm>> GetYoneticiSiteByIdAsync(int id);

        [Get("/api/v1/yonetici-siteler/by-keycloak-user/{keycloakUserId}")]
        Task<ApiResponse<List<YoneticiSiteDetailVm>>> GetYoneticiSitesByKeycloakUserIdAsync(string keycloakUserId);

        [Post("/api/v1/yonetici-siteler")]
        Task<ApiResponse<object>> CreateYoneticiSiteAsync([Body] YoneticiSiteVm dto);

        [Put("/api/v1/yonetici-siteler/{id}")]
        Task<ApiResponse<object>> UpdateYoneticiSiteAsync(int id, [Body] YoneticiSiteDetailVm dto);

        [Delete("/api/v1/yonetici-siteler/{id}")]
        Task<ApiResponse<object>> DeleteYoneticiSiteAsync(int id);
    }
}