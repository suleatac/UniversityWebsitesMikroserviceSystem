using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Dil;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IDilService
    {
        Task<ServiceResult<List<GetDilVm>>> GetDilsAsync();
    }
}
