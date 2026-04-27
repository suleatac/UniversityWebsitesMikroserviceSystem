using Microservice.Admin.ViewModels.Site;
using Refit;

namespace Microservice.Admin.Clients.SiteClients
{
    public interface ISiteClientServices
    {
        [Get("/api/v1/sites")]
        Task<ApiResponse<List<SiteGetVm>>> GetSitesAsync();

        [Get("/api/v1/sites/{siteId}")]
        Task<ApiResponse<SiteDetailGetVm>> GetSitesById(int siteId);
    }
}
