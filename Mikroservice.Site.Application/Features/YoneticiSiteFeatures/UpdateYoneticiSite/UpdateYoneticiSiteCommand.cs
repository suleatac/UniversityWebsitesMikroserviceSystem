using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.UpdateYoneticiSite
{
    public record UpdateYoneticiSiteCommand : IRequestByServiceResult
    {
        public int Id { get; init; }

        public int YoneticiTipiId { get; init; }
    }
}
