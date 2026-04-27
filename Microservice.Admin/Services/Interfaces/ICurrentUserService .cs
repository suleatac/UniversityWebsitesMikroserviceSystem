namespace Microservice.Admin.Services.Interfaces
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        string Username { get; }
        List<string> Roles { get; }
        bool IsAuthenticated { get; }
    }
}
