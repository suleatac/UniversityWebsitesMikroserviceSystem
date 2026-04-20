using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.PopupFeatures.DeletePopup;

namespace Mikroservice.Site.Api.Endpoints.PopupEndPoints.EndPoints
{
    public static class DeletePopupEndPoint
    {
        public static RouteGroupBuilder DeletePopupEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeletePopupCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeletePopup")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
