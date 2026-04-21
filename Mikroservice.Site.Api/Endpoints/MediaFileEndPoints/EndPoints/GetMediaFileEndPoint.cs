using MediatR;
using Mikroservice.Site.Application.Features.MediaFileFeatures.GetMediaFile;
using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.MediaFileEndPoints.EndPoints
{
    public static class GetMediaFileEndPoint
    {
        public static RouteGroupBuilder GetMediaFileEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (
                int siteId,
                int dilId,
                IMediator mediator) => {
                    var result = await mediator.Send(new GetMediaFilesQuery(siteId, dilId));
                    return result.ToGenericResult();
                })
            .WithName("GetMediaFiles")
            .MapToApiVersion(1.0)
            .Produces<List<MediaFile>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
