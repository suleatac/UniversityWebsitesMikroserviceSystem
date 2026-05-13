namespace Microservice.Admin.ViewModels.User
{
    public class KeycloakRoleVm
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool Composite { get; set; }
    }
}