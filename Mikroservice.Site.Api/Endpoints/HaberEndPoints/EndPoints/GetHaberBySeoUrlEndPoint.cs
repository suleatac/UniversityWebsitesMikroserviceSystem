using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.DTOs.HaberDtos;
using Mikroservice.Site.Application.Features.HaberFeatures.GetHaberBySeoUrl;

namespace Mikroservice.Site.Api.Endpoints.HaberEndPoints.EndPoints
{
    public static class GetHaberBySeoUrlEndPoint
    {
        public static RouteGroupBuilder GetHaberBySeoUrlEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/seo/{siteId:int}/{dilId:int}/{seoUrl}", async (IMediator mediator, int siteId, int dilId, string seoUrl) =>
                (await mediator.Send(new GetHaberBySeoUrlQuery(siteId, dilId, seoUrl))).ToGenericResult())
                .WithName("GetHaberBySeoUrl")
                .MapToApiVersion(1.0)
                .Produces<HaberDetailDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}