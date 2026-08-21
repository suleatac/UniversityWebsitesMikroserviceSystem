using Microservice.Web.ViewModels.Duyuru;
using Refit;

namespace Microservice.Web.Clients.DuyuruClients
{
    public interface IDuyuruClientServices
    {
        [Get("/api/v1/duyurular")]
        Task<ApiResponse<List<GetDuyuruVm>>> GetDuyurularAsync(int siteId, int dilId);

        [Get("/api/v1/duyurular/{id}")]
        Task<ApiResponse<DuyuruDetailVm>> GetDuyuruByIdAsync(int id);

       
    }
}