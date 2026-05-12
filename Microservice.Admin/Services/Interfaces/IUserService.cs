using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.User;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<string>> CreateAccount(UserAddVm model);
        Task<ServiceResult> DeleteAccount(string userId);
        Task<ServiceResult> UpdateAccount(string userId, UserUpdateVm model);
        Task<ServiceResult<List<UserListVm>>> GetUsersAsync();
        Task<ServiceResult<UserListVm>> GetUserByIdAsync(string userId);
        Task<ServiceResult<PaginatedResult<UserListVm>>> GetUsersPaginatedAsync(
    int page, int pageSize, string search, string orderBy, string orderDir);
    }
}
