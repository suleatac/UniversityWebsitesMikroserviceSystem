using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.YoneticiSiteFeatures.GetYoneticiSitesByKeycloakUserId;

namespace Mikroservice.Site.Api.Endpoints.YoneticiSiteEndPoints.EndPoints
{
    public static class GetYoneticiSitesByKeycloakUserIdEndPoint
    {
        public static RouteGroupBuilder GetYoneticiSitesByKeycloakUserIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/by-keycloak-user/{keycloakUserId}", async (
                IMediator mediator,
                string keycloakUserId) =>
            {
                var result = await mediator.Send(new GetYoneticiSitesByKeycloakUserIdQuery(keycloakUserId));
                return result.ToGenericResult();
            })
            .WithName("GetYoneticiSitesByKeycloakUserId")
            .MapToApiVersion(1.0)
            .Produces<List<Mikroservice.Site.Application.DTOs.YoneticiSiteDtos.YoneticiSiteDetailDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}