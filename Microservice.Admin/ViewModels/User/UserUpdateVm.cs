namespace Microservice.Admin.ViewModels.User
{
    public class UserUpdateVm
    {
        public string Id { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool Enabled { get; set; }
    }
}
