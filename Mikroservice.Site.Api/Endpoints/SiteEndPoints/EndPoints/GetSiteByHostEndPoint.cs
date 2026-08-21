using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.DTOs.SiteDtos;
using Mikroservice.Site.Application.Features.SiteFeatures.GetSiteByHost;

namespace Mikroservice.Site.Api.Endpoints.SiteEndPoints.EndPoints
{
    public static class GetSiteByHostEndPoint
    {
        public static RouteGroupBuilder GetSiteByHostEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/by-host/{host}", async (IMediator mediator, string host) => {
                var result = await mediator.Send(new GetSiteByHostQuery(host));
                return result.ToGenericResult();
            })
            .WithName("GetSiteByHost")
            .MapToApiVersion(1.0)
            .Produces<PageRouteDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}