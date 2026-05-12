using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.SiteFeatures.GetSite;
using Mikroservice.Site.Application.Features.YoneticiSiteFeatures.GetYoneticiSites;

namespace Mikroservice.Site.Api.Endpoints.YoneticiSiteEndPoints.EndPoints
{
    public static class GetYoneticiSitesEndPoint
    {
        public static RouteGroupBuilder GetYoneticiSitesEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IMediator mediator) => {
                var result = await mediator.Send(new GetYoneticiSitesQuery());
                return result.ToGenericResult();
            })
            .WithName("GetYoneticiSites")
            .MapToApiVersion(1.0)
            .Produces<List<Domain.Entities.Site>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
