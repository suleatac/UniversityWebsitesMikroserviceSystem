namespace Mikroservice.Site.Application.Contracts.Services
{
    public interface IUserContextService
    {
        int UserId { get; }
        string? Username { get; }
        string? TraceId { get; }
        string? IpAddress { get; }
    }
}
