using Microservice.Admin.ViewModels.SiteOzellikleri;
using Refit;

namespace Microservice.Admin.Clients.SiteOzellikleriClients
{
    public interface ISiteOzellikleriClientServices
    {
        [Get("/api/v1/site-ozellikleri/{siteId}")]
        Task<ApiResponse<SiteOzellikleriVm>> GetSiteOzellikleriAsync(int siteId);

        [Post("/api/v1/site-ozellikleri")]
        Task<ApiResponse<object>> CreateSiteOzellikleriAsync([Body] SiteOzellikleriVm dto);

        [Put("/api/v1/site-ozellikleri/{id}")]
        Task<ApiResponse<object>> UpdateSiteOzellikleriAsync(int id, [Body] SiteOzellikleriVm dto);

        [Delete("/api/v1/site-ozellikleri/{id}")]
        Task<ApiResponse<object>> DeleteSiteOzellikleriAsync(int id);
    }
}