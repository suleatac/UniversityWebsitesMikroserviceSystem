using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.HaberFeatures.DeleteHaber;

namespace Mikroservice.Site.Api.Endpoints.HaberEndPoints.EndPoints
{
    public static class DeleteHaberEndPoint
    {
        public static RouteGroupBuilder DeleteHaberEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteHaberCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeleteHaber")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
