using Microservice.Admin.ViewModels.Birim;
using Refit;

namespace Microservice.Admin.Clients.BirimClients
{
    public interface IBirimClientServices
    {
        [Get("/api/v1/birims")]
        Task<ApiResponse<List<GetBirimVm>>> GetBirimsAsync();

        [Get("/api/v1/birims/{id}")]
        Task<ApiResponse<GetBirimDetailVm>> GetBirimByIdAsync(int id);

        [Post("/api/v1/birims")]
        Task<ApiResponse<object>> CreateBirimAsync([Body] CreateBirimVm dto);

        [Put("/api/v1/birims/{id}")]
        Task<ApiResponse<object>> UpdateBirimAsync(int id, [Body] UpdateBirimVm dto);

        [Delete("/api/v1/birims/{id}")]
        Task<ApiResponse<object>> DeleteBirimAsync(int id);
    }
}
