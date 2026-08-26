using Microservice.Web.ViewModels.Dil;
using Refit;

namespace Microservice.Web.Clients.DilClients
{
    public interface IDilClientServices
    {
        [Get("/api/v1/dils")]
        Task<ApiResponse<List<GetDilVm>>> GetDilsAsync();

        [Get("/api/v1/dils/{id}")]
        Task<ApiResponse<GetDilVm>> GetDilByIdAsync(int id);
    }
}
