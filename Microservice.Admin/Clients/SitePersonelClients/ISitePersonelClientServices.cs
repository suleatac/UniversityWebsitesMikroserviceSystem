using Microservice.Admin.ViewModels.SitePersonel;
using Refit;

namespace Microservice.Admin.Clients.SitePersonelClients
{
    public interface ISitePersonelClientServices
    {
        [Get("/api/v1/site-personeller")]
        Task<ApiResponse<List<GetSitePersonelVm>>> GetSitePersonellerAsync(int siteId);

        [Get("/api/v1/site-personeller/{id}")]
        Task<ApiResponse<SitePersonelDetailVm>> GetSitePersonelByIdAsync(int id);

        [Post("/api/v1/site-personeller")]
        Task<ApiResponse<object>> CreateSitePersonelAsync([Body] CreateSitePersonelVm dto);

        [Put("/api/v1/site-personeller/{id}")]
        Task<ApiResponse<object>> UpdateSitePersonelAsync(int id, [Body] SitePersonelDetailVm dto);

        [Delete("/api/v1/site-personeller/{id}")]
        Task<ApiResponse<object>> DeleteSitePersonelAsync(int id);
    }
}