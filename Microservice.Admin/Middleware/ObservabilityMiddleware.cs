using System.Diagnostics;

namespace Microservice.Admin.Middleware
{
    public class ObservabilityMiddleware
    {
        private readonly RequestDelegate _next;

        public ObservabilityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var logger = context.RequestServices
                .GetRequiredService<ILogger<ObservabilityMiddleware>>();

            var user = context.User;

            // -----------------------------
            // TRACE ID (Guaranteed)
            // -----------------------------
            var traceId =
                Activity.Current?.TraceId.ToString()
                ?? context.TraceIdentifier
                ?? Guid.NewGuid().ToString();

            // -----------------------------
            // USER INFO (SAFE)
            // -----------------------------
            int? userId = null;

            var userIdClaim = user?.FindFirst("UserId")?.Value;
            if (int.TryParse(userIdClaim, out var parsedUserId))
            {
                userId = parsedUserId;
            }

            var username =
                user?.Identity?.IsAuthenticated == true
                    ? user.Identity.Name ?? "Anonymous"
                    : "Anonymous";

            // -----------------------------
            // SESSION SAFE ACCESS
            // -----------------------------
            int? siteId = null;

            try
            {
                siteId = context.Session?.GetInt32("CurrentSiteId");
            }
            catch
            {
                // Session middleware yoksa patlamasın
                siteId = null;
            }

            // -----------------------------
            // REQUEST INFO
            // -----------------------------
            var ip =
                context.Connection.RemoteIpAddress?.ToString()
                ?? "0.0.0.0";

            var path =
                context.Request.Path.Value ?? "/";

            // -----------------------------
            // STORE TRACE ID
            // -----------------------------
            context.Items["TraceId"] = traceId;

            // -----------------------------
            // LOG SCOPE
            // -----------------------------
            using (logger.BeginScope(new Dictionary<string, object> {
                ["TraceId"] = traceId,
                ["UserId"] = userId ?? -1,
                ["Username"] = username,
                ["SiteId"] = siteId ?? -1,
                ["IP"] = ip,
                ["Path"] = path
            }))
            {
                await _next(context);
            }
        }
    }
}