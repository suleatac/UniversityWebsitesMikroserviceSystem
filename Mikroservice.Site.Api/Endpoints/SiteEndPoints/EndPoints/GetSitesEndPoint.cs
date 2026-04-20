using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.SiteFeatures.GetSite;

namespace Mikroservice.Site.Api.Endpoints.SiteEndPoints.EndPoints
{
    public static class GetSitesEndPoint
    {
        public static RouteGroupBuilder GetSitesEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetSiteQuery());
                return result.ToGenericResult();
            })
            .WithName("GetSites")
            .MapToApiVersion(1.0)
            .Produces<List<Domain.Entities.Site>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
