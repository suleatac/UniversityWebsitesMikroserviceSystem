using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.TumPersonel;

namespace Microservice.Admin.Services.Interfaces
{
    public interface ITumPersonelService
    {
        Task<ServiceResult<List<GetPersonelVm>>> GetTumPersonelsAsync();
        Task<ServiceResult<GetPersonelVm>> GetPersonelByIdAsync(int id);
    }
}
