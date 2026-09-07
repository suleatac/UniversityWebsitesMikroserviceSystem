using Microservice.Web.ViewModels.Etkinlik;
using Refit;

namespace Microservice.Web.Clients.EtkinlikClients
{
    public interface IEtkinlikClientServices
    {

        [Get("/api/v1/etkinlikler/{id}")]
        Task<ApiResponse<EtkinlikVm>> GetEtkinlikByIdAsync(int id);

        [Get("/api/v1/etkinlikler")]
        Task<ApiResponse<List<EtkinlikVm>>> GetEtkinliklerAsync(int siteId, int dilId);


    }
}
