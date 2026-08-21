using Microservice.Web.ViewModels.Haber;
using Refit;

namespace Microservice.Web.Clients.HaberClients
{
    public interface IHaberClientServices
    {
        [Get("/api/v1/habers")]
        Task<ApiResponse<List<GetHaberVm>>> GetHabersAsync(int siteId, int dilId);

        [Get("/api/v1/habers/{id}")]
        Task<ApiResponse<HaberDetailVm>> GetHaberByIdAsync(int id);

      
    }
}
