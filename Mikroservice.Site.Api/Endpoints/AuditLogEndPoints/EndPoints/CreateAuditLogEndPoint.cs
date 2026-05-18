using MediatR;
using Microservice.Shared.Extentions;
using Microservice.Shared.Filters;
using Microsoft.AspNetCore.Mvc;
using Mikroservice.Site.Application.Features.AuditLogFeatures.CreateAuditLog;

namespace Mikroservice.Site.Api.Endpoints.AuditLogEndPoints.EndPoints
{
    public static class CreateAuditLogEndPoint
    {
        public static RouteGroupBuilder CreateAuditLogEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreateAuditLogCommand command) => {
                    var result = await mediator.Send(command);
                    return result.ToGenericResult();
                })
            .WithName("CreateAuditLog")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateAuditLogCommand>>();

            return group;
        }
    }
}
