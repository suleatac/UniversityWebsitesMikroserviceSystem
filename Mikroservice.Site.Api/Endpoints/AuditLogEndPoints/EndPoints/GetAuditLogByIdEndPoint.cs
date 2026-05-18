using MediatR;
using Mikroservice.Site.Application.Features.AuditLogFeatures.GetAuditLogById;
using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.AuditLogEndPoints.EndPoints
{
    public static class GetAuditLogByIdEndPoint
    {
        public static RouteGroupBuilder GetAuditLogByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:int}", async (
                int id,
                IMediator mediator) => {
                    var result = await mediator.Send(new GetAuditLogByIdQuery(id));
                    return result.ToGenericResult();
                })
            .WithName("GetAuditLogById")
            .MapToApiVersion(1.0)
            .Produces<AuditLog>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
