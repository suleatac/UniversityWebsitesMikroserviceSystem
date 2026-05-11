using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.SikcaSorulanSoruKategori;

namespace Microservice.Admin.Services.Interfaces
{
    public interface ISikcaSorulanSoruKategoriService
    {
        Task<ServiceResult<List<GetSikcaSorulanSoruKategoriVm>>> GetSikcaSorulanSoruKategorilerAsync();
        Task<ServiceResult<SikcaSorulanSoruKategoriVm>> GetSikcaSorulanSoruKategoriByIdAsync(int id);
        Task<ServiceResult<bool>> CreateSikcaSorulanSoruKategoriAsync(SikcaSorulanSoruKategoriVm dto);
        Task<ServiceResult<bool>> UpdateSikcaSorulanSoruKategoriAsync(SikcaSorulanSoruKategoriVm dto);
        Task<ServiceResult<bool>> DeleteSikcaSorulanSoruKategoriAsync(int id);
    }
}