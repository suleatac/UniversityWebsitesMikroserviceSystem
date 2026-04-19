using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;
using MediatR;
using Mikroservice.Site.Application.Features.EtkinlikFeatures.GetEtkinliks;

namespace Mikroservice.Site.Api.Endpoints.EtkinlikEndPoints.EndPoints
{
    public static class GetEtkinliklerEndPoint
    {
        public static RouteGroupBuilder GetEtkinliklerEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (
                int siteId,
                int dilId,
                IMediator mediator) =>
            {
                var result = await mediator.Send(new GetEtkinliksQuery(siteId, dilId));
                return result.ToGenericResult();
            })
            .WithName("GetEtkinlikler")
            .MapToApiVersion(1.0)
            .Produces<List<Etkinlik>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
