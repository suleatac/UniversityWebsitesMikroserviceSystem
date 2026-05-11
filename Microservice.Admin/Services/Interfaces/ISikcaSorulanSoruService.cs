using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.SikcaSorulanSoru;

namespace Microservice.Admin.Services.Interfaces
{
    public interface ISikcaSorulanSoruService
    {
        Task<ServiceResult<List<GetSikcaSorulanSoruVm>>> GetSikcaSorulanSorularAsync(int siteId, int dilId);
        Task<ServiceResult<SikcaSorulanSoruDetailVm>> GetSikcaSorulanSoruByIdAsync(int id);
        Task<ServiceResult<object>> CreateSikcaSorulanSoruAsync(CreateSikcaSorulanSoruVm dto);
        Task<ServiceResult<object>> UpdateSikcaSorulanSoruAsync(SikcaSorulanSoruDetailVm dto);
        Task<ServiceResult<object>> DeleteSikcaSorulanSoruAsync(int id);
    }
}