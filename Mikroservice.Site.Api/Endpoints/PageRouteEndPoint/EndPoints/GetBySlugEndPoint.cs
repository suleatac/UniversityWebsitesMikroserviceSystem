using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.DTOs.SiteDtos;
using Mikroservice.Site.Application.Features.PageRouteFeatures.GetBySlug;

namespace Mikroservice.Site.Api.Endpoints.PageRouteEndPoint.EndPoints
{
    public static class GetBySlugEndPoint
    {
        public static RouteGroupBuilder GetBySlugEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/slug/{siteId}/{slug}", async (IMediator mediator, int siteId, string slug) => {
                var result = await mediator.Send(new GetBySlugQuery(siteId, slug));
                return result.ToGenericResult();
            })
            .WithName("GetBySlug")
            .MapToApiVersion(1.0)
            .Produces<PageRouteDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
