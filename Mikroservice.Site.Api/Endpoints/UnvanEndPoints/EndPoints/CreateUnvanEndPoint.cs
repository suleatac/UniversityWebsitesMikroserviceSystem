using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.UnvanFeatures.CreateUnvan;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.UnvanEndPoints.EndPoints
{
    public static class CreateUnvanEndPoint
    {
        public static RouteGroupBuilder CreateUnvanEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreateUnvanCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("CreateUnvan")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateUnvanCommand>>();

            return group;
        }
    }
}
