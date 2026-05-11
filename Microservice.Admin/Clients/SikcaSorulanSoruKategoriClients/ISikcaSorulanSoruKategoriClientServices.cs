using Microservice.Admin.ViewModels.SikcaSorulanSoruKategori;
using Refit;

namespace Microservice.Admin.Clients.SikcaSorulanSoruKategoriClients
{
    public interface ISikcaSorulanSoruKategoriClientServices
    {
        [Get("/api/v1/sss-kategoriler")]
        Task<ApiResponse<List<GetSikcaSorulanSoruKategoriVm>>> GetSikcaSorulanSoruKategorilerAsync();

        [Get("/api/v1/sss-kategoriler/{id}")]
        Task<ApiResponse<SikcaSorulanSoruKategoriVm>> GetSikcaSorulanSoruKategoriByIdAsync(int id);

        [Post("/api/v1/sss-kategoriler")]
        Task<ApiResponse<object>> CreateSikcaSorulanSoruKategoriAsync([Body] SikcaSorulanSoruKategoriVm dto);

        [Put("/api/v1/sss-kategoriler/{id}")]
        Task<ApiResponse<object>> UpdateSikcaSorulanSoruKategoriAsync(int id, [Body] SikcaSorulanSoruKategoriVm dto);

        [Delete("/api/v1/sss-kategoriler/{id}")]
        Task<ApiResponse<object>> DeleteSikcaSorulanSoruKategoriAsync(int id);
    }
}