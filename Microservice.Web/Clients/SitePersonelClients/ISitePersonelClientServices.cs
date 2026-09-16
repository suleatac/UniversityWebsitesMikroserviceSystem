using Microservice.Web.ViewModels.SitePersonel;
using Refit;

namespace Microservice.Web.Clients.SitePersonelClients
{
    public interface ISitePersonelClientServices
    {
        [Get("/api/v1/site-personeller/{id}")]
        Task<ApiResponse<PersonelDetailVm>> GetPersonelByIdAsync(int id);

        [Get("/api/v1/site-personeller")]
        Task<ApiResponse<List<GetPersonelVm>>> GetPersonelListAsync(int siteId);
        [Get("/api/v1/site-personeller/seo/{siteId}/{seoUrl}")]
        Task<ApiResponse<PersonelDetailVm>> GetPersonelBySeoUrlAsync(int siteId, string seoUrl);
    }
}
