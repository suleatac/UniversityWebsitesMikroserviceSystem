using Microservice.Web.ViewModels.Content;
using Refit;

namespace Microservice.Web.Clients.BilgiClients
{
    public interface IBilgiClientServices
    {
        [Get("/api/v1/bilgiler/{id}")]
        Task<ApiResponse<ContentDetailVm>> GetBilgiByIdAsync(int id);
    }
}
