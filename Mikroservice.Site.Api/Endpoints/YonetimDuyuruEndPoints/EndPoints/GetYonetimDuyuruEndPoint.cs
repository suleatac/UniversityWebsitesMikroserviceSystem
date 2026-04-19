using MediatR;
using Microservice.Shared.Extentions;
using Microservice.Site.Application.Features.YonetimDuyuruFeatures.GetYonetimDuyurulari;
using Microservice.Site.Domain.Entities;

namespace Microservice.Site.Api.Endpoints.YonetimDuyuruEndPoints.EndPoints
{
    public static class GetYonetimDuyuruEndPoint
    {
        public static RouteGroupBuilder GetYonetimDuyurulariEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IMediator mediator) => {
                var result = await mediator.Send(new GetYonetimDuyurulariQuery());
                return result.ToGenericResult();
            })
             .WithName("GetYonetimDuyurulari")
             .MapToApiVersion(1.0)
             .Produces<List<YonetimDuyuru>>(StatusCodes.Status200OK)
             .Produces(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status404NotFound)
             .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
