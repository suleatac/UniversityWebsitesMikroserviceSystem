using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Dil;

namespace Microservice.Web.Services.Interfaces
{
    public interface IDilService
    {
        Task<ServiceResult<List<GetDilVm>>> GetDilsAsync();
        Task<ServiceResult<GetDilVm>> GetDilByIdAsync(int id);
    }
}
