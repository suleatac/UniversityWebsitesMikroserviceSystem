using Microservice.Web.ViewModels.SikcaSorulanSoru;
using Refit;

namespace Microservice.Web.Clients.SikcaSorulanSoruClients
{
    public interface ISikcaSorulanSoruClientServices
    {
        [Get("/api/v1/sss")]
        Task<ApiResponse<List<SikcaSorulanSoruVm>>> GetSikcaSorulanSorularAsync(int siteId, int dilId);
    }
}
