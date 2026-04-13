using MediatR;
using Microservice.Yonetici.Application.Features.YoneticiTipiFeatures.GetYoneticiTipleri;
using Microservice.Shared.Extentions;

namespace Microservice.Yonetici.Api.Endpoints.YoneticiTipiEndPoints.EndPoints
{
    public static class GetYoneticiTipleriEndPoint
    {
        public static RouteGroupBuilder GetYoneticiTipleriEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IMediator mediator) => {
                var result = await mediator.Send(new GetYoneticiTipleriQuery());
                return result.ToGenericResult();
            })
             .WithName("GetYoneticiTipleri")
             .MapToApiVersion(1.0)
             .Produces<Guid>(StatusCodes.Status201Created)
             .Produces(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status404NotFound)
             .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
