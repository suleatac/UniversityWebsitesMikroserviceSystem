using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.YoneticiTipi;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IYoneticiTipiService
    {
        Task<ServiceResult<List<GetYoneticiTipiVm>>> GetYoneticiTipleriAsync();
    }
}