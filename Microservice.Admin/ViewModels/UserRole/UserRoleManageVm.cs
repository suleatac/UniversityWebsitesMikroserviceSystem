using Microservice.Admin.ViewModels.User;

namespace Microservice.Admin.ViewModels.UserRole
{
    public class UserRoleManageVm
    {
        public UserListVm User { get; set; } = new();
        public List<KeycloakRoleVm> AvailableRoles { get; set; } = new();
        public List<KeycloakRoleVm> AssignedRoles { get; set; } = new();
    }
}