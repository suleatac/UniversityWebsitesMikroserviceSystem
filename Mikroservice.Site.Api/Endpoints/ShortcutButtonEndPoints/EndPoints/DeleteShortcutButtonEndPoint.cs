using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.ShortcutButtonFeatures.DeleteShortcutButton;

namespace Mikroservice.Site.Api.Endpoints.ShortcutButtonEndPoints.EndPoints
{
    public static class DeleteShortcutButtonEndPoint
    {
        public static RouteGroupBuilder DeleteShortcutButtonEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteShortcutButtonCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeleteShortcutButton")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
