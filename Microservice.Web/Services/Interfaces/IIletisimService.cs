using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Iletisim;

namespace Microservice.Web.Services.Interfaces
{
    public interface IIletisimService
    {
        Task<ServiceResult> GonderAsync(IletisimMesajiVm model);
    }
}
