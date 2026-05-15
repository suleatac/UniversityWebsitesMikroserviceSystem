using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.DTOs.YonetimDuyuru;
using Mikroservice.Site.Application.Features.YonetimDuyuruFeatures.GetYonetimDuyuruDetail;
using System.Security.Claims;

namespace Mikroservice.Site.Api.Endpoints.YonetimDuyuruEndPoints.EndPoints
{
    public static class GetYonetimDuyuruDetailEndPoint
    {
        public static RouteGroupBuilder GetYonetimDuyuruDetailEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id}/detail", async (IMediator mediator, int id, HttpContext httpContext) => {
                var keycloakUserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
                var result = await mediator.Send(new GetYonetimDuyuruDetailQuery(id, keycloakUserId));
                return result.ToGenericResult();
            })
             .WithName("GetYonetimDuyuruDetail")
             .MapToApiVersion(1.0)
             .Produces<YonetimDuyuruDetailDto>(StatusCodes.Status200OK)
             .Produces(StatusCodes.Status404NotFound)
             .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
