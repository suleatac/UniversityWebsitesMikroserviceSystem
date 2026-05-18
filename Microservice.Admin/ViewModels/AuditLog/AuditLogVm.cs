namespace Microservice.Admin.ViewModels.AuditLog
{
    public class AuditLogVm
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        public string? TraceId { get; set; }

        public string Username { get; set; } = default!;

        public string Action { get; set; } = default!;
    }
}
