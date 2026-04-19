using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.HaberFeatures.CreateHaber;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.HaberEndPoints.EndPoints
{
    public static class CreateHaberEndPoint
    {
        public static RouteGroupBuilder CreateHaberEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreateHaberCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("CreateHaber")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateHaberCommand>>();

            return group;
        }
    }
}
