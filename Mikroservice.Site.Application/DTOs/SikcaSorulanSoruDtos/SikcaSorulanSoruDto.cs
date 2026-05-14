namespace Mikroservice.Site.Application.DTOs.SikcaSorulanSoruDtos
{
    public class SikcaSorulanSoruDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; } // 🔥 nullable
        public int SiteId { get; set; }
        public int DilId { get; set; }

        public string Soru { get; set; } = default!;
        public string Cevap { get; set; } = default!;

        public int Sira { get; set; }

        // 🔥 SEO (çok önemli)
        public string? SeoUrl { get; set; }
        public List<SikcaSorulanSoruDto> Children { get; set; } = new();
    }
}
