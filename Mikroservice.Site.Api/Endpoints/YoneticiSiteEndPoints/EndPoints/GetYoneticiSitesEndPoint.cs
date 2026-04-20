using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;
using MediatR;
using Mikroservice.Site.Application.Features.YoneticiSiteFeatures.GetYoneticiSites;

namespace Mikroservice.Site.Api.Endpoints.YoneticiSiteEndPoints.EndPoints
{
    public static class GetYoneticiSitesEndPoint
    {
        public static RouteGroupBuilder GetYoneticiSitesEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (
                int siteId,
                IMediator mediator) =>
            {
                var result = await mediator.Send(
                    new GetYoneticiSitesQuery(siteId)
                );

                return result.ToGenericResult();
            })
            .WithName("GetYoneticiSites")
            .MapToApiVersion(1.0)
            .Produces<List<YoneticiSite>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
