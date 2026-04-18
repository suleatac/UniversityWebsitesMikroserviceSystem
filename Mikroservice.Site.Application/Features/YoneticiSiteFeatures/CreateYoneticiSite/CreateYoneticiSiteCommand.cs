using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.CreateYoneticiSite
{
    public record CreateYoneticiSiteCommand : IRequestByServiceResult
    {
        public string KeycloakUserId { get; init; } = default!;

        public int SiteId { get; init; }

        public int YoneticiTipiId { get; init; }
    }
}
