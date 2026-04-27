using MediatR;
using Mikroservice.Site.Application.Features.BirimFeatures.GetBirim;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.DTOs;

namespace Mikroservice.Site.Api.Endpoints.BirimEndPoints.EndPoints
{
    public static class GetBirimsEndPoint
    {
        public static RouteGroupBuilder GetBirimsEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetBirimQuery());
                return result.ToGenericResult();
            })
            .WithName("GetBirims")
            .MapToApiVersion(1.0)
            .Produces<List<BirimDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
