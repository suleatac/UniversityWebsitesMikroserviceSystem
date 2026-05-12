namespace Microservice.Admin.Services.Interfaces
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        string KeycloakUserId { get; }
        string Username { get; }
        List<string> Roles { get; }
        bool IsAuthenticated { get; }
        bool IsAdmin { get; }
    }
}
