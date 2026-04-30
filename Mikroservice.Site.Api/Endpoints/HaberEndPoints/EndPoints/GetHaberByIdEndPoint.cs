using MediatR;
using Mikroservice.Site.Application.DTOs.HaberDtos;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.HaberFeatures.GetHaberById;

namespace Mikroservice.Site.Api.Endpoints.HaberEndPoints.EndPoints
{
    public static class GetHaberByIdEndPoint
    {
        public static RouteGroupBuilder GetHaberByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id}", async (IMediator mediator, int id) => {
                var result = await mediator.Send(new GetHaberByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetHaberById")
            .MapToApiVersion(1.0)
            .Produces<HaberDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
