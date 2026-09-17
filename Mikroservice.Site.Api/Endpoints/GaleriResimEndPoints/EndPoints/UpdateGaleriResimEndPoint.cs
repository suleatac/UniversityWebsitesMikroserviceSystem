using Microservice.Shared.Filters;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mikroservice.Site.Application.Features.GaleriResimFeatures.UpdateGaleriResim;

namespace Mikroservice.Site.Api.Endpoints.GaleriResimEndPoints.EndPoints
{
    public static class UpdateGaleriResimEndPoint
    {
        public static RouteGroupBuilder UpdateGaleriResimEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPut("/{id:int}", async (
                int id,
                [FromServices] IMediator mediator,
                [FromBody] UpdateGaleriResimCommand command) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("Id uyuşmuyor");

                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("UpdateGaleriResim")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<UpdateGaleriResimCommand>>();

            return group;
        }
    }
}
