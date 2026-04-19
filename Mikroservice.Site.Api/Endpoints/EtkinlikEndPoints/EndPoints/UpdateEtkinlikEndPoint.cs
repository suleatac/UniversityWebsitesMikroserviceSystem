using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.EtkinlikFeatures.UpdateEtkinlik;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.EtkinlikEndPoints.EndPoints
{
    public static class UpdateEtkinlikEndPoint
    {
        public static RouteGroupBuilder UpdateEtkinlikEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPut("/{id:int}", async (
                int id,
                [FromServices] IMediator mediator,
                [FromBody] UpdateEtkinlikCommand command) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("Id uyuşmuyor");

                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("UpdateEtkinlik")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<UpdateEtkinlikCommand>>();

            return group;
        }
    }
}
