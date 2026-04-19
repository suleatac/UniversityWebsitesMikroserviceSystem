using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;
using MediatR;
using Mikroservice.Site.Application.Features.HaberFeatures.GetHabers;

namespace Mikroservice.Site.Api.Endpoints.HaberEndPoints.EndPoints
{
    public static class GetHaberlerEndPoint
    {
        public static RouteGroupBuilder GetHaberlerEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (
                int siteId,
                int dilId,
                IMediator mediator) =>
            {
                var result = await mediator.Send(new GetHabersQuery(siteId, dilId));
                return result.ToGenericResult();
            })
            .WithName("GetHaberler")
            .MapToApiVersion(1.0)
            .Produces<List<Haber>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
