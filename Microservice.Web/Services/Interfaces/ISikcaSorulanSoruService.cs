using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.SikcaSorulanSoru;

namespace Microservice.Web.Services.Interfaces
{
    public interface ISikcaSorulanSoruService
    {
        // Site API'den agac yapisiyla (kategori -> sorular) gelen SSS listesi.
        Task<ServiceResult<List<SikcaSorulanSoruVm>>> GetSikcaSorulanSorularAsync(int siteId, int dilId);
    }
}
