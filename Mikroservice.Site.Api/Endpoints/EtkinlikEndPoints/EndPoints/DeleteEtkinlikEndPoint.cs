using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.EtkinlikFeatures.DeleteEtkinlik;

namespace Mikroservice.Site.Api.Endpoints.EtkinlikEndPoints.EndPoints
{
    public static class DeleteEtkinlikEndPoint
    {
        public static RouteGroupBuilder DeleteEtkinlikEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteEtkinlikCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeleteEtkinlik")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
