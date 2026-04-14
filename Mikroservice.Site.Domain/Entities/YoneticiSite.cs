using Microservice.Site.Domain.Entities;

namespace Mikroservice.Site.Domain.Entities
{
    public class YoneticiSite
    {
        public int YoneticiId { get; set; }
        public Yonetici Yonetici { get; set; } = default!;

        public int SiteId { get; set; }
        public Site Site { get; set; } = default!;

        public int YoneticiTipiId { get; set; }
        public YoneticiTipi YoneticiTipi { get; set; } = default!;

        public bool IsDeleted { get; set; }
    }
}
