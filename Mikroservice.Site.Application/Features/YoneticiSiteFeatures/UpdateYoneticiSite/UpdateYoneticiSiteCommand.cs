using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.UpdateYoneticiSite
{
    public record UpdateYoneticiSiteCommand : IRequestByServiceResult<UpdateYoneticiSiteResponse>
    {
        public int Id { get; init; }
        public int SiteId { get; init; }
        public int YoneticiTipiId { get; init; }
        public int PersonelId { get; init; }
    }
}
