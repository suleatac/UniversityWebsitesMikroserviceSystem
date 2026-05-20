using MediatR;
using Mikroservice.Site.Application.DTOs.AuditLogDtos;
using Mikroservice.Site.Application.Features.AuditLogFeatures.GetAuditLogDailyStats;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.AuditLogEndPoints.EndPoints
{
    public static class GetAuditLogDailyStatsEndPoint
    {
        public static RouteGroupBuilder GetAuditLogDailyStatsEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/daily-stats", async (
                DateTime startDate,
                DateTime endDate,
                IMediator mediator) => {
                    var query = new GetAuditLogDailyStatsQuery(startDate, endDate);
                    var result = await mediator.Send(query);
                    return result.ToGenericResult();
                })
            .WithName("GetAuditLogDailyStats")
            .MapToApiVersion(1.0)
            .Produces<List<AuditLogDailyStatDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
