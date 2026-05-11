using Mikroservice.Site.Application.DTOs.YonetimDuyuru;
using Microservice.Shared.Extentions;
using MediatR;
using Mikroservice.Site.Application.Features.YonetimDuyuruFeatures.GetYonetimDuyuruById;

namespace Mikroservice.Site.Api.Endpoints.YonetimDuyuruEndPoints.EndPoints
{
    public static class GetYonetimDuyuruByIdEndPoint
    {
        public static RouteGroupBuilder GetYonetimDuyuruByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id}", async (IMediator mediator, int id) => {
                var result = await mediator.Send(new GetYonetimDuyuruByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetYonetimDuyuruById")
            .MapToApiVersion(1.0)
            .Produces<YonetimDuyuruDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
