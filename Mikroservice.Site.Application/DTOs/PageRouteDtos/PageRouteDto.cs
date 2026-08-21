using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.DTOs.SiteDtos
{
    public class PageRouteDto
    {
        public long Id { get; set; }

        public int SiteId { get; set; }

        public string Slug { get; set; } = null!;
        public int? SeoMetadataId { get; set; }

        public int PageTypeId { get; set; }

        public int? ContentId { get; set; } //ait olduğu tablodaki id (örn. Duyuru id , Haber id, Etkinlik id gibi).

        public DateTime CreatedAt { get; set; }

        public SeoMetadata SeoMetadata { get; set; } = null!;
    }
}
