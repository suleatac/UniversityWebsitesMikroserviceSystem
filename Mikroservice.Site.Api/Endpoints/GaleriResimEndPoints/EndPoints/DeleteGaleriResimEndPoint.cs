using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.GaleriResimFeatures.DeleteGaleriResim;

namespace Mikroservice.Site.Api.Endpoints.GaleriResimEndPoints.EndPoints
{
    public static class DeleteGaleriResimEndPoint
    {
        public static RouteGroupBuilder DeleteGaleriResimEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteGaleriResimCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeleteGaleriResim")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
