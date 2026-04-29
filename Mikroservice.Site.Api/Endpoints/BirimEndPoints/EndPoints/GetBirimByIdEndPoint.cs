using MediatR;
using Mikroservice.Site.Application.DTOs.BirimDtos;
using Mikroservice.Site.Application.Features.BirimFeatures.GetBirimById;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.BirimEndPoints.EndPoints
{
    public static class GetBirimByIdEndPoint
    {
        public static RouteGroupBuilder GetBirimByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id}", async (IMediator mediator, int id) => {
                var result = await mediator.Send(new GetBirimByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetBirimById")
            .MapToApiVersion(1.0)
            .Produces<BirimDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
