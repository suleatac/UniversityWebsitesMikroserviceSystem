using Microservice.Admin.Attributes;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.AuditLog;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Microservice.Admin.Filters
{
    public class AuditLogFilter : IAsyncActionFilter
    {
        private readonly IAuditLogService _auditLogService;
        private readonly ILogger<AuditLogFilter> _logger;

        public AuditLogFilter(IAuditLogService auditLogService, ILogger<AuditLogFilter> logger)
        {
            _auditLogService = auditLogService;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sw = Stopwatch.StartNew();
            var resultContext = await next();
            sw.Stop();

            var skip = context.ActionDescriptor.EndpointMetadata
           .Any(x => x is SkipAuditAttribute);

            if (skip)
                return;

            // Sadece başarılı POST (Create/Edit/Delete) işlemlerini logla
            if (resultContext.Exception != null)
                return;

            if (context.HttpContext.Request.Method != "POST")
                return;

            try
            {
                var httpContext = context.HttpContext;
                var user = httpContext.User;
                var auditAttribute = context.ActionDescriptor.EndpointMetadata.OfType<AuditLogAttribute>().FirstOrDefault();

                // Controller ve Action ismini al
                var controllerName = context.RouteData.Values["controller"]?.ToString() ?? "Unknown";
                var actionName = context.RouteData.Values["action"]?.ToString() ?? "Unknown";

                // Action tipini belirle (Create/Edit/Delete)
                var actionType = actionName switch
                {
                    "Create" or "CreateConfirmed" => "Create",
                    "Edit" or "Update" => "Update",
                    "Delete" or "DeleteConfirmed" or "Remove" => "Delete",
                    _ => actionName
                };

                // EntityId'yi route'dan veya action argümanlarından dene
                var entityId = context.RouteData.Values["id"]?.ToString();
                if (string.IsNullOrEmpty(entityId))
                {
                    // Action argümanlarında "id" parametresini ara
                    foreach (var kvp in context.ActionArguments)
                    {
                        if (kvp.Key.Equals("id", StringComparison.OrdinalIgnoreCase))
                        {
                            entityId = kvp.Value?.ToString();
                            break;
                        }
                        // ViewModel içinde Id property'sini ara
                        if (kvp.Value != null)
                        {
                            var idProp = kvp.Value.GetType().GetProperty("Id");
                            if (idProp != null)
                            {
                                entityId = idProp.GetValue(kvp.Value)?.ToString();
                                break;
                            }
                        }
                    }
                }

                // IP adresini X-Forwarded-For üzerinden al (proxy arkasında ise)
                var ipAddress = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                                ?? httpContext.Connection.RemoteIpAddress?.ToString()
                                ?? "0.0.0.0";
                if (!string.IsNullOrWhiteSpace(ipAddress) && ipAddress.Contains(','))
                {
                    ipAddress = ipAddress.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).FirstOrDefault();
                }

                // Description: AuditLogAttribute'den geliyorsa onu kullan, yoksa otomatik oluştur
                var description = auditAttribute?.Description
                                  ?? $"{controllerName} - {actionType}";

                // Action adını okunabilir formata çevir
                var actionDisplayName = $"{controllerName}/{actionName}";

                var log = new CreateAuditLogVm
                {
                    UserId = user.FindFirst("sub")?.Value ?? "-1",
                    Username = user.Identity?.Name?? "Anonymous",
                    Action = actionType,
                    TraceId = Activity.Current?.TraceId.ToString()
                              ?? httpContext.TraceIdentifier,
                    IpAddress = ipAddress,
                    SiteId = httpContext.Session.GetInt32("CurrentSiteId"),
                    EntityName = controllerName,
                    EntityId = entityId,
                    Description = description
                };

                await _auditLogService.CreateAuditLogAsync(log);

                _logger.LogInformation(
                    "AuditLog kaydedildi | Controller: {Controller} | Action: {Action} | EntityId: {EntityId} | Süre: {ElapsedMs}ms",
                    controllerName, actionType, entityId ?? "-", sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                // Audit log yazma hatası uygulamayı çökertmesin
                _logger.LogError(ex, "AuditLog kaydedilemedi: {Message}", ex.Message);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var skip = context.ActionDescriptor.EndpointMetadata
                .Any(x => x is SkipAuditAttribute);

            if (skip)
                return;

            // audit logic
        }
    }
}
