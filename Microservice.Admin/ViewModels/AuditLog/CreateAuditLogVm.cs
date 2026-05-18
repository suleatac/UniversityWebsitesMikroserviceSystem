namespace Microservice.Admin.ViewModels.AuditLog
{
    public class CreateAuditLogVm
    {
        public string? UserId { get; set; } 

        public string Username { get; set; } = default!;

        public string Action { get; set; } = default!;
        public string? TraceId { get; set; }

        public string? EntityName { get; set; }

        public string? EntityId { get; set; }

        public string? Description { get; set; }

        public string IpAddress { get; set; } = default!;

        public int? SiteId { get; set; }
    }
}
