using Microservice.Admin.ViewModels.Personel;
using Refit;

namespace Microservice.Admin.Clients.PersonelClients
{
    public interface IPersonelClientServices
    {
        [Get("/api/v1/personels")]
        Task<ApiResponse<List<GetPersonelVm>>> GetPersonellerAsync();

        [Get("/api/v1/personels/{id}")]
        Task<ApiResponse<PersonelDetailVm>> GetPersonelByIdAsync(int id);
    }
}
