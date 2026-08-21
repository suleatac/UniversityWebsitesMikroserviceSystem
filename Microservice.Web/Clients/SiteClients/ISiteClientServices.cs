using Microservice.Web.ViewModels.Site;
using Refit;

namespace Microservice.Web.Clients.SiteClients
{
    public interface ISiteClientServices
    {
        [Get("/api/v1/sites")]
        Task<ApiResponse<List<SiteGetVm>>> GetSitesAsync();

        [Get("/api/v1/sites/{id}")]
        Task<ApiResponse<SiteDetailGetVm>> GetSiteByIdAsync(int id);

      
    }
}
