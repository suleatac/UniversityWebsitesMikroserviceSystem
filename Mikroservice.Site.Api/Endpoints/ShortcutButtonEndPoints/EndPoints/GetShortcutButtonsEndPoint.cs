using Microservice.Shared.Extentions;
using MediatR;
using Mikroservice.Site.Application.Features.ShortcutButtonFeatures.GetShortcutButtons;
using Mikroservice.Site.Application.DTOs.ShortcutButtonDtos;

namespace Mikroservice.Site.Api.Endpoints.ShortcutButtonEndPoints.EndPoints
{
    public static class GetShortcutButtonsEndPoint
    {
        public static RouteGroupBuilder GetShortcutButtonsEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (
                int siteId,
                int dilId,
                IMediator mediator) =>
            {
                var result = await mediator.Send(new GetShortcutButtonsQuery(siteId, dilId));
                return result.ToGenericResult();
            })
            .WithName("GetShortcutButtons")
            .MapToApiVersion(1.0)
            .Produces<List<ShortcutButtonDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
