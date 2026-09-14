using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.DTOs.BilgiDtos;
using Mikroservice.Site.Application.Features.BilgiFeatures.GetBilgiBySeoUrl;

namespace Mikroservice.Site.Api.Endpoints.BilgiEndPoints.EndPoints
{
    public static class GetBilgiBySeoUrlEndPoint
    {
        public static RouteGroupBuilder GetBilgiBySeoUrlEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/seo/{siteId:int}/{dilId:int}/{seoUrl}", async (IMediator mediator, int siteId, int dilId, string seoUrl) =>
                (await mediator.Send(new GetBilgiBySeoUrlQuery(siteId, dilId, seoUrl))).ToGenericResult())
                .WithName("GetBilgiBySeoUrl")
                .MapToApiVersion(1.0)
                .Produces<BilgiDetailDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}
