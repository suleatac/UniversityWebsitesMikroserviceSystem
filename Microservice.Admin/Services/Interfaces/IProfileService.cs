using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Profile;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ServiceResult<ProfileVm>> GetCurrentUserProfileAsync(string keycloakUserId);
        Task<ServiceResult> UpdateUserProfileAsync(string keycloakUserId, ProfileVm model);
    }
}