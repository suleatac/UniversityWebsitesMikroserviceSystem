using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.HaberFeatures.UpdateHaber;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.HaberEndPoints.EndPoints
{
    public static class UpdateHaberEndPoint
    {
        public static RouteGroupBuilder UpdateHaberEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPut("/{id:int}", async (
                int id,
                [FromServices] IMediator mediator,
                [FromBody] UpdateHaberCommand command) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("Id uyuşmuyor");

                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("UpdateHaber")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<UpdateHaberCommand>>();

            return group;
        }
    }
}
