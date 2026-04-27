using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.User;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult> CreateAccount(UserAddVm model);
    }
}
