namespace Mikroservice.Site.Domain.Entities
{
    /// <summary>
    /// Carousel tipindeki PageBlock'un slaytlari (resim + opsiyonel video linki).
    /// </summary>
    public class PageBlockMedia
    {
        public int Id { get; set; }

        public int PageBlockId { get; set; }

        public string ResimUrl { get; set; } = default!;

        public string? VideoUrl { get; set; }

        public int Sira { get; set; }

        // Navigation
        public PageBlock PageBlock { get; set; } = default!;
    }
}
