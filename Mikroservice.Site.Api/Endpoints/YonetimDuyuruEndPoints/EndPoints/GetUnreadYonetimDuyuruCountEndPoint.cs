using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.YonetimDuyuruFeatures.GetUnreadYonetimDuyuruCount;
using System.Security.Claims;

namespace Mikroservice.Site.Api.Endpoints.YonetimDuyuruEndPoints.EndPoints
{
    public static class GetUnreadYonetimDuyuruCountEndPoint
    {
        public static RouteGroupBuilder GetUnreadYonetimDuyuruCountEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/unread-count", async (IMediator mediator, HttpContext httpContext) => {
                var keycloakUserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
                var result = await mediator.Send(new GetUnreadYonetimDuyuruCountQuery(keycloakUserId));
                return result.ToGenericResult();
            })
             .WithName("GetUnreadYonetimDuyuruCount")
             .MapToApiVersion(1.0)
             .Produces<int>(StatusCodes.Status200OK)
             .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
