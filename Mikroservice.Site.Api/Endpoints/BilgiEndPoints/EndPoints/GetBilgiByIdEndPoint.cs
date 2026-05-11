using MediatR;
using Mikroservice.Site.Application.DTOs.BilgiDtos;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.BilgiFeatures.GetBilgiById;

namespace Mikroservice.Site.Api.Endpoints.BilgiEndPoints.EndPoints
{
    public static class GetBilgiByIdEndPoint
    {
        public static RouteGroupBuilder GetBilgiByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:int}", async (IMediator mediator, int id) =>
            {
                var result = await mediator.Send(new GetBilgiByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetBilgiById")
            .MapToApiVersion(1.0)
            .Produces<BilgiDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}