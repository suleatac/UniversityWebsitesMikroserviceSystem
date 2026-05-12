using Microservice.Site.Domain.Entities;

namespace Mikroservice.Site.Domain.Entities
{
    public class YoneticiSite
    {
        public int Id { get; set; }

        public string KeycloakUserId { get; set; } = default!;

        public int SiteId { get; set; }

        public int YoneticiTipiId { get; set; }

        public bool IsDeleted { get; set; }
        public YoneticiTipi YoneticiTipi { get; set; } = default!;
        public Site Site { get; set; } = default!;
    }
}
