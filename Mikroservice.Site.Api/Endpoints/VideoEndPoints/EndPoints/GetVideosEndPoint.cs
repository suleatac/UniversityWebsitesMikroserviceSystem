using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;
using MediatR;
using Mikroservice.Site.Application.Features.VideoFeatures.GetVideos;

namespace Mikroservice.Site.Api.Endpoints.VideoEndPoints.EndPoints
{
    public static class GetVideosEndPoint
    {
        public static RouteGroupBuilder GetVideosEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (
                int siteId,
                int dilId,
                IMediator mediator) =>
            {
                if (siteId <= 0 || dilId <= 0)
                    return Results.BadRequest("Geçersiz siteId veya dilId");

                var result = await mediator.Send(
                    new GetVideosQuery(siteId, dilId)
                );

                return result.ToGenericResult();
            })
            .WithName("GetVideos")
            .MapToApiVersion(1.0)
            .Produces<List<Video>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
