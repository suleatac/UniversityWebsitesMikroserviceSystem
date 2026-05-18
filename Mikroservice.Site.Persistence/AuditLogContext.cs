namespace Mikroservice.Site.Persistence
{
    public static class AuditLogContext
    {
        public static string UserId { get; set; } = "-1";

        public static string Username { get; set; } = "SYSTEM";

        public static string TraceId { get; set; } = Guid.NewGuid().ToString();

        public static string IpAddress { get; set; } = "0.0.0.0";
    }
}
