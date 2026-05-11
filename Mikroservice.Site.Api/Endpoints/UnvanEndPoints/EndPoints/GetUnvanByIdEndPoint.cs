using Mikroservice.Site.Application.DTOs.UnvanDtos;
using Microservice.Shared.Extentions;
using MediatR;
using Mikroservice.Site.Application.Features.UnvanFeatures.GetUnvanById;

namespace Mikroservice.Site.Api.Endpoints.UnvanEndPoints.EndPoints
{
    public static class GetUnvanByIdEndPoint
    {
        public static RouteGroupBuilder GetUnvanByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id}", async (IMediator mediator, int id) => {
                var result = await mediator.Send(new GetUnvanByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetUnvanById")
            .MapToApiVersion(1.0)
            .Produces<UnvanDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
