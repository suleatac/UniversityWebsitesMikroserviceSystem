using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Haber;

namespace Microservice.Web.Services.Interfaces
{
    public interface IHaberService
    {
        Task<ServiceResult<List<GetHaberVm>>> GetHabersAsync(int siteId, int dilId);
        Task<ServiceResult<HaberDetailVm>> GetHaberByIdAsync(int id);
        
    }
}
