using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.ShortcutButtonFeatures.UpdateShortcutButton;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.ShortcutButtonEndPoints.EndPoints
{
    public static class UpdateShortcutButtonEndPoint
    {
        public static RouteGroupBuilder UpdateShortcutButtonEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPut("/{id:int}", async (
                int id,
                [FromServices] IMediator mediator,
                [FromBody] UpdateShortcutButtonCommand command) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("Id uyuşmuyor");

                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("UpdateShortcutButton")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<UpdateShortcutButtonCommand>>();

            return group;
        }
    }
}
