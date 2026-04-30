using Microservice.Admin.ViewModels.Site;
using Refit;

namespace Microservice.Admin.Clients.SiteClients
{
    public interface ISiteClientServices
    {
        [Get("/api/v1/sites")]
        Task<ApiResponse<List<SiteGetVm>>> GetSitesAsync();

        [Get("/api/v1/sites/{id}")]
        Task<ApiResponse<SiteDetailGetVm>> GetSiteByIdAsync(int id);

        [Post("/api/v1/sites")]
        Task<ApiResponse<object>> CreateSiteAsync([Body] CreateSiteVm dto);

        [Put("/api/v1/sites/{id}")]
        Task<ApiResponse<object>> UpdateSiteAsync(int id, [Body] SiteDetailGetVm dto);

        [Delete("/api/v1/sites/{id}")]
        Task<ApiResponse<object>> DeleteSiteAsync(int id);
    }
}
