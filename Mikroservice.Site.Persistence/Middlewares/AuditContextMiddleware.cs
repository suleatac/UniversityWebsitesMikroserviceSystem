using Microsoft.AspNetCore.Http;
using Mikroservice.Site.Persistence;
using System.Security.Claims;

public class AuditContextMiddleware
{
    private readonly RequestDelegate _next;

    public AuditContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var user = context.User;

            AuditLogContext.UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1";

            AuditLogContext.Username =
                user?.Identity?.IsAuthenticated == true
                    ? user.Identity.Name ?? "SYSTEM"
                    : "SYSTEM";

            AuditLogContext.TraceId =
                context.TraceIdentifier ?? Guid.NewGuid().ToString();

            AuditLogContext.IpAddress =
                context.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";

            await _next(context);
        }
        finally
        {
            AuditLogContext.UserId = "-1";
            AuditLogContext.Username = "SYSTEM";
            AuditLogContext.TraceId = Guid.NewGuid().ToString();
            AuditLogContext.IpAddress = "0.0.0.0";
        }
    }
}