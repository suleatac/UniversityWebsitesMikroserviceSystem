using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.MenuFeatures.DeleteMenu;

namespace Mikroservice.Site.Api.Endpoints.MenuEndPoints.EndPoints
{
    public static class DeleteMenuEndPoint
    {
        public static RouteGroupBuilder DeleteMenuEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteMenuCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeleteMenu")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
