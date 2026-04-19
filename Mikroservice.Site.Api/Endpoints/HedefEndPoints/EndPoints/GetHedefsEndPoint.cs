using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.HedefFeatures.GetHedef;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Api.Endpoints.HedefEndPoints.EndPoints
{
    public static class GetHedefsEndPoint
    {
        public static RouteGroupBuilder GetHedefsEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetHedefsQuery());
                return result.ToGenericResult();
            })
            .WithName("GetHedefs")
            .MapToApiVersion(1.0)
            .Produces<List<Hedef>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
