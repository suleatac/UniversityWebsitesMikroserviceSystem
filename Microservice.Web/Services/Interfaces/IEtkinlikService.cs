using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Etkinlik;

namespace Microservice.Web.Services.Interfaces
{
    public interface IEtkinlikService
    {
        Task<ServiceResult<List<GetEtkinlikVm>>> GetEtkinliklerAsync(int siteId, int dilId);
        Task<ServiceResult<EtkinlikDetailVm>> GetEtkinlikByIdAsync(int id);
    }
}
