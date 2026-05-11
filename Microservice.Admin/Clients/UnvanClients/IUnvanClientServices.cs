using Microservice.Admin.ViewModels.Unvan;
using Refit;

namespace Microservice.Admin.Clients.UnvanClients
{
    public interface IUnvanClientServices
    {
        [Get("/api/v1/unvans")]
        Task<ApiResponse<List<GetUnvanVm>>> GetUnvansAsync();

        [Get("/api/v1/unvans/{id}")]
        Task<ApiResponse<UnvanVm>> GetUnvanByIdAsync(int id);

        [Post("/api/v1/unvans")]
        Task<ApiResponse<object>> CreateUnvanAsync([Body] UnvanVm dto);

        [Put("/api/v1/unvans/{id}")]
        Task<ApiResponse<object>> UpdateUnvanAsync(int id, [Body] UnvanVm dto);

        [Delete("/api/v1/unvans/{id}")]
        Task<ApiResponse<object>> DeleteUnvanAsync(int id);
    }
}
