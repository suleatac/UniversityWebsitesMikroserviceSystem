using Microservice.Web.ViewModels.Bilgi;
using Refit;

namespace Microservice.Web.Clients.BilgiClients
{
    public interface IBilgiClientServices
    {
        [Get("/api/v1/bilgis/{id}")]
        Task<ApiResponse<BilgiVm>> GetBilgiByIdAsync(int id);

        [Get("/api/v1/bilgis")]
        Task<ApiResponse<List<BilgiVm>>> GetBilgisAsync(int siteId, int dilId);
    }
}
