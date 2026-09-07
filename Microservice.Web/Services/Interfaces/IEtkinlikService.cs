using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Etkinlik;

namespace Microservice.Web.Services.Interfaces
{
    public interface IEtkinlikService
    {
        Task<ServiceResult<List<EtkinlikVm>>> GetEtkinliklerAsync(int siteId, int dilId);
        Task<ServiceResult<EtkinlikVm>> GetEtkinlikByIdAsync(int id);
    }
}
