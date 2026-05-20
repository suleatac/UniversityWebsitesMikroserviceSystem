using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.AuditLogDtos;

namespace Mikroservice.Site.Application.Features.AuditLogFeatures.GetAuditLogDailyStats
{
    public record GetAuditLogDailyStatsQuery(
        DateTime StartDate,
        DateTime EndDate
    ) : IRequestByServiceResult<List<AuditLogDailyStatDto>>;
}
