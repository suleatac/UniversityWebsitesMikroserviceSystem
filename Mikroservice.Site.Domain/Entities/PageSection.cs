namespace Mikroservice.Site.Domain.Entities
{
    /// <summary>
    /// Ana sayfa dinamik bolumu. Her bolum bir <section> olarak render edilir;
    /// arka plan rengi/gorseli ve icindeki container (PageBlock) listesi
    /// admin panelinden yonetilir.
    /// </summary>
    public class PageSection
    {
        public int Id { get; set; }

        public int SiteId { get; set; }
        public int DilId { get; set; }

        public string Baslik { get; set; } = default!;

        /// <summary>Hex color veya css rengi (orn: #0a3d62).</summary>
        public string? BackgroundColor { get; set; }

        /// <summary>Varsa section parallax arka plan gorsel adresi.</summary>
        public string? BackgroundImageUrl { get; set; }

        /// <summary>Bolumun sayfa uzerindeki sirasi (kucuk ustte).</summary>
        public int Sira { get; set; }

        public bool Yayinda { get; set; } = true;

        public DateTime OlusturulmaTarihi { get; set; }

        public bool IsDeleted { get; set; } = false;

        // Navigation
        public Site Site { get; set; } = default!;
        public Dil Dil { get; set; } = default!;
        public ICollection<PageBlock> Blocks { get; set; } = new List<PageBlock>();
    }
}
