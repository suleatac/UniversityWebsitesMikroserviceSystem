using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.BirimFeatures.DeleteBirim;

namespace Mikroservice.Site.Api.Endpoints.BirimEndPoints.EndPoints
{
    public static class DeleteBirimEndPoint
    {
        public static RouteGroupBuilder DeleteBirimEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteBirimCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeleteBirim")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
