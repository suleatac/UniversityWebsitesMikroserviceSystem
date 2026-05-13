namespace Mikroservice.Site.Application.DTOs.PopupDtos
{
    public class PopupDetailDto
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public string Baslik { get; set; } = default!;
        public string KisaAciklama { get; set; } = default!;
        public string IcerikMetni { get; set; } = default!;
        public string? Link { get; set; }
        public string? ResimUrl { get; set; }
        public int GosterimSayisi { get; set; }
        public DateTime YayimTarihi { get; set; }
        public DateTime EklemeTarihi { get; set; }
        public DateTime? BaslamaTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public string? SeoUrl { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public bool TamEkranMi { get; set; }
        public int GosterimSuresiSaniye { get; set; }
        public bool CookieIleTekrarGosterme { get; set; }
    }
}