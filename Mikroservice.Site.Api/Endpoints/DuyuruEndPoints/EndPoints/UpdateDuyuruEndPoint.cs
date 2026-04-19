using MediatR;
using Microservice.Shared.Filters;
using Microsoft.AspNetCore.Mvc;
using Mikroservice.Site.Application.Features.DuyuruFeatures.UpdateDuyuru;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.DuyuruEndPoints.EndPoints
{
    public static class UpdateDuyuruEndPoint
    {
        public static RouteGroupBuilder UpdateDuyuruEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPut("/{id:int}", async (
                int id,
                [FromServices] IMediator mediator,
                [FromBody] UpdateDuyuruCommand command) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("Id uyuşmuyor");

                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("UpdateDuyuru")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<UpdateDuyuruCommand>>();

            return group;
        }
    }
}
