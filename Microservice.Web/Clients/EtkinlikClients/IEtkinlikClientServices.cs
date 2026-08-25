using Microservice.Web.ViewModels.Content;
using Refit;

namespace Microservice.Web.Clients.EtkinlikClients
{
    public interface IEtkinlikClientServices
    {
        [Get("/api/v1/etkinlikler/{id}")]
        Task<ApiResponse<ContentDetailVm>> GetEtkinlikByIdAsync(int id);
    }
}
