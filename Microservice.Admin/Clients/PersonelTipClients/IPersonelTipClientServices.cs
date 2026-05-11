using Microservice.Admin.ViewModels.PersonelTip;
using Refit;

namespace Microservice.Admin.Clients.PersonelTipClients
{
    public interface IPersonelTipClientServices
    {
        [Get("/api/v1/personel-tipler")]
        Task<ApiResponse<List<GetPersonelTipVm>>> GetPersonelTiplerAsync();

        [Get("/api/v1/personel-tipler/{id}")]
        Task<ApiResponse<PersonelTipVm>> GetPersonelTipByIdAsync(int id);

        [Post("/api/v1/personel-tipler")]
        Task<ApiResponse<object>> CreatePersonelTipAsync([Body] PersonelTipVm dto);

        [Put("/api/v1/personel-tipler/{id}")]
        Task<ApiResponse<object>> UpdatePersonelTipAsync(int id, [Body] PersonelTipVm dto);

        [Delete("/api/v1/personel-tipler/{id}")]
        Task<ApiResponse<object>> DeletePersonelTipAsync(int id);
    }
}