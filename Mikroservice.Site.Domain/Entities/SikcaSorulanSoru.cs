namespace Mikroservice.Site.Domain.Entities
{
    public class SikcaSorulanSoru
    {
        public int Id { get; set; }

        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int KategoriId { get; set; }

        public string Soru { get; set; } = default!;
        public string Cevap { get; set; } = default!;

        public int Sira { get; set; }

        public bool IsDeleted { get; set; } = false;

        // 🔥 SEO (çok önemli)
        public string? SeoUrl { get; set; }

        // NAVIGATION
        public Site Site { get; set; } = default!;
        public Dil Dil { get; set; } = default!;
        public SikcaSorulanSoruKategori Kategori { get; set; } = default!;
    }
}
