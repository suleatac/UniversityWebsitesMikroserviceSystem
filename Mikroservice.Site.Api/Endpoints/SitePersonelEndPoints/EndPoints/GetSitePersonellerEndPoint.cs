using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;
using MediatR;
using Mikroservice.Site.Application.Features.SitePersonelFeatures.GetSitePersonels;

namespace Mikroservice.Site.Api.Endpoints.SitePersonelEndPoints.EndPoints
{
    public static class GetSitePersonellerEndPoint
    {
        public static RouteGroupBuilder GetSitePersonellerEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (
                int siteId,
                IMediator mediator) =>
            {
                if (siteId <= 0)
                    return Results.BadRequest("Geçersiz SiteId");

                var result = await mediator.Send(
                    new GetSitePersonelQuery(siteId)
                );

                return result.ToGenericResult();
            })
            .WithName("GetSitePersoneller")
            .MapToApiVersion(1.0)
            .Produces<List<SitePersonel>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
