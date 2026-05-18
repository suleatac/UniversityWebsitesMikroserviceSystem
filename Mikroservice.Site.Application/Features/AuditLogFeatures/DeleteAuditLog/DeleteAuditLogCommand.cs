using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.AuditLogFeatures.DeleteAuditLog
{
    public record DeleteAuditLogCommand(int Id) : IRequestByServiceResult;

}
