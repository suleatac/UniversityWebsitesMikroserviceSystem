using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;
using MediatR;
using Mikroservice.Site.Application.Features.SiteOzellikleriFeatures.GetSiteOzellikleri;

namespace Mikroservice.Site.Api.Endpoints.SiteOzellikleriEndPoints.EndPoints
{
    public static class GetSiteOzellikleriEndPoint
    {
        public static RouteGroupBuilder GetSiteOzellikleriEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{siteId:int}", async (
                int siteId,
                IMediator mediator) =>
            {
                if (siteId <= 0)
                    return Results.BadRequest("Geçersiz SiteId");

                var result = await mediator.Send(new GetSiteOzellikleriQuery(siteId));
                return result.ToGenericResult();
            })
            .WithName("GetSiteOzellikleri")
            .MapToApiVersion(1.0)
            .Produces<SiteOzellikleri>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
