using Microservice.Admin.ViewModels.Dil;
using Refit;

namespace Microservice.Admin.Clients.DilClients
{
    public interface IDilClientServices
    {
        [Get("/api/v1/dils")]
        Task<ApiResponse<List<GetDilVm>>> GetDilsAsync();
    }
}
