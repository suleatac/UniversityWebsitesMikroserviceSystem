using MediatR;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.AuditLogDtos;
using Mikroservice.Site.Application.Features.AuditLogFeatures.GetPaginatedAuditLog;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.AuditLogEndPoints.EndPoints
{
    public static class GetPaginatedAuditLogEndPoint
    {
        public static RouteGroupBuilder GetPaginatedAuditLogEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/paginated", async (
                int page,
                int pageSize,
                string? search,
                string? orderBy,
                string? orderDir,
                IMediator mediator) => {
                    var query = new GetPaginatedAuditLogQuery(page, pageSize, search, orderBy, orderDir);
                    var result = await mediator.Send(query);
                    return result.ToGenericResult();
                })
            .WithName("GetPaginatedAuditLog")
            .MapToApiVersion(1.0)
            .Produces<PaginatedResult<AuditLogDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
