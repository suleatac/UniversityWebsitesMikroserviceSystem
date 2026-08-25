using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Content;

namespace Microservice.Web.Services.Interfaces
{
    public interface IEtkinlikService
    {
        Task<ServiceResult<ContentDetailVm>> GetEtkinlikByIdAsync(int id);
    }
}
