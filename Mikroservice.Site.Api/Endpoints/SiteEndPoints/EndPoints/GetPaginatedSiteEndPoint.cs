using MediatR;
using Mikroservice.Site.Application.Features.SiteFeatures.GetPaginatedSite;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.DTOs.SiteDtos;

namespace Mikroservice.Site.Api.Endpoints.SiteEndPoints.EndPoints
{
    public static class GetPaginatedSiteEndPoint
    {
        public static RouteGroupBuilder GetPaginatedSiteEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/paginated", async (IMediator mediator) => {
                var result = await mediator.Send(new GetPaginatedSiteQuery());
                return result.ToGenericResult();
            })
            .WithName("GetPaginatedSites")
            .MapToApiVersion(1.0)
            .Produces<SitePaginatedResult<SiteDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
