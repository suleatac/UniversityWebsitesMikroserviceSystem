using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.VideoFeatures.DeleteVideo;

namespace Mikroservice.Site.Api.Endpoints.VideoEndPoints.EndPoints
{
    public static class DeleteVideoEndPoint
    {
        public static RouteGroupBuilder DeleteVideoEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteVideoCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeleteVideo")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
