using MediatR;
using Mikroservice.Site.Application.DTOs.VideoDtos;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.VideoFeatures.GetVideoById;

namespace Mikroservice.Site.Api.Endpoints.VideoEndPoints.EndPoints
{
    public static class GetVideoByIdEndPoint
    {
        public static RouteGroupBuilder GetVideoByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:int}", async (IMediator mediator, int id) =>
            {
                var result = await mediator.Send(new GetVideoByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetVideoById")
            .MapToApiVersion(1.0)
            .Produces<VideoDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}