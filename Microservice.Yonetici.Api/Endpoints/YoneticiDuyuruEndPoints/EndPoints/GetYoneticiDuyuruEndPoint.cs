using MediatR;
using Microservice.Yonetici.Application.Features.YoneticiDuyuruFeatures.GetYoneticiDuyurulari;
using Microservice.Shared.Extentions;

namespace Microservice.Yonetici.Api.Endpoints.YoneticiDuyuruEndPoints.EndPoints
{
    public static class GetYoneticiDuyuruEndPoint
    {
        public static RouteGroupBuilder GetYoneticiDuyurulariEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IMediator mediator) => {
                var result = await mediator.Send(new GetYoneticiDuyurulariQuery());
                return result.ToGenericResult();
            })
             .WithName("GetYoneticiDuyurulari")
             .MapToApiVersion(1.0)
             .Produces<Guid>(StatusCodes.Status201Created)
             .Produces(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status404NotFound)
             .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
