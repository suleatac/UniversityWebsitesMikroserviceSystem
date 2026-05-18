using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.AuditLogDtos;

namespace Mikroservice.Site.Application.Features.AuditLogFeatures.GetAuditLogById
{
    public record GetAuditLogByIdQuery(int Id) : IRequestByServiceResult<AuditLogDetailDto>;

}
