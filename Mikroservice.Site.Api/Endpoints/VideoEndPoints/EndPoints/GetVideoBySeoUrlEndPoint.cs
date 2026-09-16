using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.DTOs.VideoDtos;
using Mikroservice.Site.Application.Features.VideoFeatures.GetVideoBySeoUrl;

namespace Mikroservice.Site.Api.Endpoints.VideoEndPoints.EndPoints
{
    public static class GetVideoBySeoUrlEndPoint
    {
        public static RouteGroupBuilder GetVideoBySeoUrlEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/seo/{siteId:int}/{dilId:int}/{seoUrl}", async (IMediator mediator, int siteId, int dilId, string seoUrl) =>
                (await mediator.Send(new GetVideoBySeoUrlQuery(siteId, dilId, seoUrl))).ToGenericResult())
                .WithName("GetVideoBySeoUrl")
                .MapToApiVersion(1.0)
                .Produces<VideoDetailDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}
 