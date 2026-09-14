using MediatR;
using Mikroservice.Site.Application.DTOs.EtkinlikDtos;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.EtkinlikFeatures.GetEtkinlikBySeoUrl;

namespace Mikroservice.Site.Api.Endpoints.EtkinlikEndPoints.EndPoints
{
    public static class GetEtkinlikBySeoUrlEndPoint
    {
        public static RouteGroupBuilder GetEtkinlikBySeoUrlEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/seo/{siteId:int}/{dilId:int}/{seoUrl}", async (IMediator mediator, int siteId, int dilId, string seoUrl) =>
                (await mediator.Send(new GetEtkinlikBySeoUrlQuery(siteId, dilId, seoUrl))).ToGenericResult())
                .WithName("GetEtkinlikBySeoUrl")
                .MapToApiVersion(1.0)
                .Produces<EtkinlikDetailDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}
