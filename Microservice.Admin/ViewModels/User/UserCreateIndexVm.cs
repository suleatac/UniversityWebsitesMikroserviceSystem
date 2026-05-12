using Microservice.Admin.ViewModels.TumPersonel;
using Microservice.Admin.ViewModels.UserRole;

namespace Microservice.Admin.ViewModels.User
{
    public class UserCreateIndexVm
    {
        public UserAddVm UserAdd { get; set; } = new();
        public List<GetPersonelVm> TumPersoneller { get; set; } = new();
        public List<KeycloakRoleVm> AvailableRoles { get; set; } = new();
    }
}