using Microservice.Admin.ViewModels.TumPersonel;
using Refit;

namespace Microservice.Admin.Clients.TumPersonelClients
{
    public interface ITumPersonelClientService
    {

        [Get("/api/v1/personels")]
        Task<ApiResponse<List<GetPersonelVm>>> GetPersonelsAsync();

        [Get("/api/v1/personels/{id}")]
        Task<ApiResponse<GetPersonelVm>> GetPersonelByIdAsync(int id);
    }
}
