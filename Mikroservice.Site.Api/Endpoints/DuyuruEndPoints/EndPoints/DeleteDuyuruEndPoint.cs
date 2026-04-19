using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.DuyuruFeatures.DeleteDuyuru;

namespace Mikroservice.Site.Api.Endpoints.DuyuruEndPoints.EndPoints
{
    public static class DeleteDuyuruEndPoint
    {
        public static RouteGroupBuilder DeleteDuyuruEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteDuyuruCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeleteDuyuru")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
