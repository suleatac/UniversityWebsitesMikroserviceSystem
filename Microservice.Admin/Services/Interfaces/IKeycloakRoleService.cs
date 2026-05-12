using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.UserRole;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IKeycloakRoleService
    {
        /// <summary>
        /// Keycloak realm'indeki tüm rolleri getirir
        /// </summary>
        Task<ServiceResult<List<KeycloakRoleVm>>> GetRealmRolesAsync();

        /// <summary>
        /// Belirli bir kullanıcının sahip olduğu realm rollerini getirir
        /// </summary>
        Task<ServiceResult<List<KeycloakRoleVm>>> GetUserRolesAsync(string userId);

        /// <summary>
        /// Kullanıcıya realm rolü atar
        /// </summary>
        Task<ServiceResult> AssignRoleToUserAsync(string userId, string roleName);

        /// <summary>
        /// Kullanıcıya birden fazla realm rolü atar
        /// </summary>
        Task<ServiceResult> AssignRolesToUserAsync(string userId, List<string> roleNames);

        /// <summary>
        /// Kullanıcıdan realm rolünü kaldırır
        /// </summary>
        Task<ServiceResult> RemoveRoleFromUserAsync(string userId, string roleName);

        /// <summary>
        /// Kullanıcıdan birden fazla realm rolünü kaldırır
        /// </summary>
        Task<ServiceResult> RemoveRolesFromUserAsync(string userId, List<string> roleNames);

        /// <summary>
        /// Kullanıcının Admin rolüne sahip olup olmadığını kontrol eder
        /// </summary>
        Task<bool> IsUserInRoleAsync(string userId, string roleName);
    }
}
