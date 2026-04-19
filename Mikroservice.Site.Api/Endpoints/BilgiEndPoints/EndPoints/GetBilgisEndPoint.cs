using MediatR;
using Mikroservice.Site.Application.Features.BilgiFeatures.GetBilgis;
using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.BilgiEndPoints.EndPoints
{
    public static class GetBilgisEndPoint
    {
        public static RouteGroupBuilder GetBilgisEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (
                int siteId,
                int dilId,
                IMediator mediator) =>
            {
                var result = await mediator.Send(new GetBilgisQuery(siteId, dilId));
                return result.ToGenericResult();
            })
            .WithName("GetBilgis")
            .MapToApiVersion(1.0)
            .Produces<List<Bilgi>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
