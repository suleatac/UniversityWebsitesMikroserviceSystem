using MediatR;
using Microservice.Shared.Filters;
using Microsoft.AspNetCore.Mvc;
using Mikroservice.Site.Application.Features.BirimFeatures.UpdateBirim;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.BirimEndPoints.EndPoints
{
    public static class UpdateBirimEndPoint
    {
        public static RouteGroupBuilder UpdateBirimEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPut("/{id:int}", async (
                int id,
                [FromServices] IMediator mediator,
                [FromBody] UpdateBirimCommand command) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("Id uyuşmuyor");

                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("UpdateBirim")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<UpdateBirimCommand>>();

            return group;
        }
    }
}
