using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.YoneticiSiteDtos;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.GetYoneticiSitesByKeycloakUserId
{
    public record GetYoneticiSitesByKeycloakUserIdQuery(string KeycloakUserId) : IRequestByServiceResult<List<YoneticiSiteDetailDto>>;
}