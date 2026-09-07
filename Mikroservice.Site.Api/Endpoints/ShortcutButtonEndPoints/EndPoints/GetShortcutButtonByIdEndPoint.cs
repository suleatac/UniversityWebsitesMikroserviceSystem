using MediatR;
using Mikroservice.Site.Application.DTOs.ShortcutButtonDtos;
using Mikroservice.Site.Application.Features.ShortcutButtonFeatures.GetShortcutButtonById;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.ShortcutButtonEndPoints.EndPoints
{
    public static class GetShortcutButtonByIdEndPoint
    {
        public static RouteGroupBuilder GetShortcutButtonByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:int}", async (IMediator mediator, int id) =>
            {
                var result = await mediator.Send(new GetShortcutButtonByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetShortcutButtonById")
            .MapToApiVersion(1.0)
            .Produces<ShortcutButtonDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
