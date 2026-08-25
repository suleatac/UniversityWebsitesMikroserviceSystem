using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.DTOs.DuyuruDtos;
using Mikroservice.Site.Application.Features.DuyuruFeatures.GetDuyuruBySeoUrl;

namespace Mikroservice.Site.Api.Endpoints.DuyuruEndPoints.EndPoints
{
    public static class GetDuyuruBySeoUrlEndPoint
    {
        public static RouteGroupBuilder GetDuyuruBySeoUrlEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/seo/{siteId:int}/{dilId:int}/{seoUrl}", async (IMediator mediator, int siteId, int dilId, string seoUrl) =>
                (await mediator.Send(new GetDuyuruBySeoUrlQuery(siteId, dilId, seoUrl))).ToGenericResult())
                .WithName("GetDuyuruBySeoUrl")
                .MapToApiVersion(1.0)
                .Produces<DuyuruDetailDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}