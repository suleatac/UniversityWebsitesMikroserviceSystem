using MediatR;
using Mikroservice.Site.Application.Features.DuyuruFeatures.GetDuyurus;
using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.DuyuruEndPoints.EndPoints
{
    public static class GetDuyurularEndPoint
    {
        public static RouteGroupBuilder GetDuyurularEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (
                int siteId,
                int dilId,
                IMediator mediator) =>
            {
                var result = await mediator.Send(new GetDuyurusQuery(siteId, dilId));
                return result.ToGenericResult();
            })
            .WithName("GetDuyurular")
            .MapToApiVersion(1.0)
            .Produces<List<Duyuru>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
