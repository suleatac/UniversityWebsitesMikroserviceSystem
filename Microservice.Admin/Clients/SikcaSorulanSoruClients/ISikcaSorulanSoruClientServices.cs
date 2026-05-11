using Microservice.Admin.ViewModels.SikcaSorulanSoru;
using Refit;

namespace Microservice.Admin.Clients.SikcaSorulanSoruClients
{
    public interface ISikcaSorulanSoruClientServices
    {
        [Get("/api/v1/sss")]
        Task<ApiResponse<List<GetSikcaSorulanSoruVm>>> GetSikcaSorulanSorularAsync(int siteId, int dilId);

        [Get("/api/v1/sss/{id}")]
        Task<ApiResponse<SikcaSorulanSoruDetailVm>> GetSikcaSorulanSoruByIdAsync(int id);

        [Post("/api/v1/sss")]
        Task<ApiResponse<object>> CreateSikcaSorulanSoruAsync([Body] CreateSikcaSorulanSoruVm dto);

        [Put("/api/v1/sss/{id}")]
        Task<ApiResponse<object>> UpdateSikcaSorulanSoruAsync(int id, [Body] SikcaSorulanSoruDetailVm dto);

        [Delete("/api/v1/sss/{id}")]
        Task<ApiResponse<object>> DeleteSikcaSorulanSoruAsync(int id);
    }
}