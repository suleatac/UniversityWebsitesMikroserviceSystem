using MediatR;
using Mikroservice.Site.Application.Features.AuditLogFeatures.DeleteAuditLog;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.AuditLogEndPoints.EndPoints
{
    public static class DeleteAuditLogEndPoint
    {
        public static RouteGroupBuilder DeleteAuditLogEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) => {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteAuditLogCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeleteAuditLog")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }

    }
}
