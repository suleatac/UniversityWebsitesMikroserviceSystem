using Microservice.Web.ViewModels.Bilgi;
using Refit;

namespace Microservice.Web.Clients.BilgiClients
{
    public interface IBilgiClientServices
    {
        [Get("/api/v1/bilgis/{id}")]
        Task<ApiResponse<BilgiDetailVm>> GetBilgiByIdAsync(int id);

        [Get("/api/v1/bilgis")]
        Task<ApiResponse<List<GetBilgiVm>>> GetBilgisAsync(int siteId, int dilId);


        [Get("/api/v1/bilgis/seo/{siteId}/{dilId}/{seoUrl}")]
        Task<ApiResponse<BilgiDetailVm>> GetBilgiBySeoUrlAsync(int siteId, int dilId, string seoUrl);
    }
}
