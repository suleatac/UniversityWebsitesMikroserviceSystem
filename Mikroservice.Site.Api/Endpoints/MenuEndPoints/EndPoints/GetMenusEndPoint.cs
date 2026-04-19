using Mikroservice.Site.Application.Contracts.DTOs;
using Microservice.Shared.Extentions;
using MediatR;
using Mikroservice.Site.Application.Features.MenuFeatures.GetMenus;

namespace Mikroservice.Site.Api.Endpoints.MenuEndPoints.EndPoints
{
    public static class GetMenusEndPoint
    {
        public static RouteGroupBuilder GetMenusEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (
                int siteId,
                int dilId,
                IMediator mediator) =>
            {
                var result = await mediator.Send(new GetMenuQuery(siteId, dilId));
                return result.ToGenericResult();
            })
            .WithName("GetMenus")
            .MapToApiVersion(1.0)
            .Produces<List<MenuDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
