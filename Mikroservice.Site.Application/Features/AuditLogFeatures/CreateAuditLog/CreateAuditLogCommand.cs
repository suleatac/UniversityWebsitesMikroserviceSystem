using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.AuditLogFeatures.CreateAuditLog
{
    public record CreateAuditLogCommand : IRequestByServiceResult<CreateAuditLogResponse>
    {
        public string? UserId { get; set; }
        public string Username { get; set; } = default!;

        public string Action { get; set; } = default!;

        public string? EntityName { get; set; }
        public string? TraceId { get; set; }

        public string? EntityId { get; set; }

        public string? Description { get; set; }

        public string IpAddress { get; set; } = default!;

    }
}
