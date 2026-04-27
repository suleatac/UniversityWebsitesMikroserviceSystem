using MediatR;
using Mikroservice.Site.Application.Features.SiteFeatures.GetSiteById;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.DTOs.SiteDtos;

namespace Mikroservice.Site.Api.Endpoints.SiteEndPoints.EndPoints
{
    public static class GetSiteByIdEndPoint
    {
        public static RouteGroupBuilder GetSiteByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id}", async (IMediator mediator, int id) => {
                var result = await mediator.Send(new GetSiteByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetSiteById")
            .MapToApiVersion(1.0)
            .Produces<SiteDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
