using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.MenuFeatures.UpdateMenu;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.MenuEndPoints.EndPoints
{
    public static class UpdateMenuEndPoint
    {
        public static RouteGroupBuilder UpdateMenuEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPut("/{id:int}", async (
                int id,
                [FromServices] IMediator mediator,
                [FromBody] UpdateMenuCommand command) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("Id uyuşmuyor");

                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("UpdateMenu")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<UpdateMenuCommand>>();

            return group;
        }
    }
}
