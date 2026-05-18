using Microservice.Shared;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.AuditLogDtos;

namespace Mikroservice.Site.Application.Features.AuditLogFeatures.GetPaginatedAuditLog
{
    public record GetPaginatedAuditLogQuery(
        int Page = 1,
        int PageSize = 10,
        string? Search = null,
        string? OrderBy = "Id",
        string? OrderDir = "desc"
    ) : IRequestByServiceResult<PaginatedResult<AuditLogDto>>;
}
