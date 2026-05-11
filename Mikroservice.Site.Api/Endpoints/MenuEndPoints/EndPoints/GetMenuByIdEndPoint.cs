using MediatR;
using Mikroservice.Site.Application.DTOs.MenuDtos;
using Mikroservice.Site.Application.Features.MenuFeatures.GetMenuById;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.MenuEndPoints.EndPoints
{
    public static class GetMenuByIdEndPoint
    {
        public static RouteGroupBuilder GetMenuByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id}", async (IMediator mediator, int id) => {
                var result = await mediator.Send(new GetMenuByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetMenuById")
            .MapToApiVersion(1.0)
            .Produces<MenuDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}