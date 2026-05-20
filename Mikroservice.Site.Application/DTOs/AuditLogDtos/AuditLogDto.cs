namespace Mikroservice.Site.Application.DTOs.AuditLogDtos
{
    public record AuditLogDto
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        public string Username { get; set; } = default!;
        public string? TraceId { get; set; }

        public string Action { get; set; } = default!;

        public string? EntityName { get; set; }

        public string? EntityId { get; set; }

        public string? Description { get; set; }

        public string IpAddress { get; set; } = default!;

        public DateTime CreatedAt { get; set; }
    }
}
