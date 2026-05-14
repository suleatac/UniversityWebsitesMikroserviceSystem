using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.SikcaSorulanSoru;

namespace Microservice.Admin.Services.Interfaces
{
    public interface ISikcaSorulanSoruService
    {
        Task<ServiceResult<List<GetSikcaSorulanSoruVm>>> GetSikcaSorulanSorularAsync(int siteId, int dilId);
        Task<ServiceResult<SikcaSorulanSoruDetailVm>> GetSikcaSorulanSoruByIdAsync(int id);
        Task<ServiceResult<bool>> CreateSikcaSorulanSoruAsync(CreateSikcaSorulanSoruVm dto);
        Task<ServiceResult<bool>> UpdateSikcaSorulanSoruAsync(SikcaSorulanSoruDetailVm dto);
        Task<ServiceResult<bool>> DeleteSikcaSorulanSoruAsync(int id);
    }
}
