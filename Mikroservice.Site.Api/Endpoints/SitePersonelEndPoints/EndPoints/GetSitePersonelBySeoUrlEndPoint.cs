using Mikroservice.Site.Application.DTOs.SitePersonelDtos;
using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.SitePersonelFeatures.GetSitePersonelBySeoUrl;

namespace Mikroservice.Site.Api.Endpoints.SitePersonelEndPoints.EndPoints
{
    public static class GetSitePersonelBySeoUrlEndPoint
    {
        public static RouteGroupBuilder GetSitePersonelBySeoUrlEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/seo/{siteId:int}/{seoUrl}", async (IMediator mediator, int siteId,  string seoUrl) =>
                (await mediator.Send(new GetSitePersonelBySeoUrlQuery(siteId, seoUrl))).ToGenericResult())
                .WithName("GetSitePersonelBySeoUrl")
                .MapToApiVersion(1.0)
                .Produces<SitePersonelDetailDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            return group;
        }

    }
}
