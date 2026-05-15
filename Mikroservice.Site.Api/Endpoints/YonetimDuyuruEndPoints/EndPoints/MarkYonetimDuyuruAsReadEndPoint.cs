using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.YonetimDuyuruFeatures.MarkYonetimDuyuruAsRead;
using System.Security.Claims;

namespace Mikroservice.Site.Api.Endpoints.YonetimDuyuruEndPoints.EndPoints
{
    public static class MarkYonetimDuyuruAsReadEndPoint
    {
        public static RouteGroupBuilder MarkYonetimDuyuruAsReadEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/{id}/mark-read", async (IMediator mediator, int id, HttpContext httpContext) => {
                var keycloakUserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
                var result = await mediator.Send(new MarkYonetimDuyuruAsReadCommand(id, keycloakUserId));
                return result.ToGenericResult();
            })
             .WithName("MarkYonetimDuyuruAsRead")
             .MapToApiVersion(1.0)
             .Produces<bool>(StatusCodes.Status200OK)
             .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
