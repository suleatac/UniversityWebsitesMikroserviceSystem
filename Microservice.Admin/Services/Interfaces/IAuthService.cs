using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.SignIn;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult> AuthenticateAsync(SignInVm signInViewModel);
    }
}
