using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.DTOs.GaleriResimDtos;
using Mikroservice.Site.Application.Features.GaleriResimFeatures.GetGaleriResimBySeoUrl;

namespace Mikroservice.Site.Api.Endpoints.GaleriResimEndPoints.EndPoints
{
    public static class GetGaleriResimBySeoUrlEndPoint
    {
        public static RouteGroupBuilder GetGaleriResimBySeoUrlEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/seo/{siteId:int}/{dilId:int}/{seoUrl}", async (IMediator mediator, int siteId, int dilId, string seoUrl) =>
                (await mediator.Send(new GetGaleriResimBySeoUrlQuery(siteId, dilId, seoUrl))).ToGenericResult())
                .WithName("GetGaleriResimBySeoUrl")
                .MapToApiVersion(1.0)
                .Produces<GaleriResimDetailDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}
