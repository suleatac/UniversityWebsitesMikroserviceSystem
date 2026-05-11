using Microservice.Admin.ViewModels.YoneticiTipi;
using Refit;

namespace Microservice.Admin.Clients.YoneticiTipiClients
{
    public interface IYoneticiTipiClientServices
    {
        [Get("/api/v1/yoneticiTipi")]
        Task<ApiResponse<List<GetYoneticiTipiVm>>> GetYoneticiTipleriAsync();
    }
}