namespace Mikroservice.Site.Domain.Entities
{
    public class PageRoute
    {
        public long Id { get; set; }

        public int SiteId { get; set; }

        public string Slug { get; set; } = null!;

        /// <summary>
        /// Bu PageRoute'a ait SEO metadata kaydının Id'si.
        /// </summary>
        public int? SeoMetadataId { get; set; }

        public int PageTypeId { get; set; }

        /// <summary>
        /// İlgili içerik tablosundaki kayıt Id'si.
        /// Örneğin DuyuruId, HaberId, EtkinlikId vb.
        /// </summary>
        public int? ContentId { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public Site Site { get; set; } = null!;

        public PageType PageType { get; set; } = null!;

        /// <summary>
        /// Bu PageRoute'a ait SEO metadata.
        /// </summary>
        public SeoMetadata? SeoMetadata { get; set; } = null!;
    }
}