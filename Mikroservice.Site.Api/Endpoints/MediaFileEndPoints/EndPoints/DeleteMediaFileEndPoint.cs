using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.MediaFileFeatures.DeleteMediaFile;

namespace Mikroservice.Site.Api.Endpoints.MediaFileEndPoints.EndPoints
{
    public static class DeleteMediaFileEndPoint
    {
        public static RouteGroupBuilder DeleteMediaFileEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) => {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteMediaFileCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeleteMediaFile")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
