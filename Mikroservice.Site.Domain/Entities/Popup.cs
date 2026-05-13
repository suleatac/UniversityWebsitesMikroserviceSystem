namespace Mikroservice.Site.Domain.Entities
{
    public class Popup
    {
        public int Id { get; set; }

        public int SiteId { get; set; }

        public string Baslik { get; set; } = default!;
        public string KisaAciklama { get; set; } = default!;
        public string IcerikMetni { get; set; } = default!;

        public string? Link { get; set; }
        public string? ResimUrl { get; set; }
        public int GosterimSayisi { get; set; } = 0;

        public DateTime YayimTarihi { get; set; }
        public DateTime EklemeTarihi { get; set; }

        public DateTime? BaslamaTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }

        public bool IsDeleted { get; set; } = false;

        // SEO
        public string? SeoUrl { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }

        // Popup-specific
        public bool TamEkranMi { get; set; } = false;
        public int GosterimSuresiSaniye { get; set; }
        public bool CookieIleTekrarGosterme { get; set; } = true;

        // Navigation
        public Site Site { get; set; } = default!;
    }
}
