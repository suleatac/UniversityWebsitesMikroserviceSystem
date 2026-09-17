using Mikroservice.Site.Application.DTOs.PageTypeDtos;

namespace Mikroservice.Site.Application.DTOs.GaleriResimDtos
{
    public class GaleriResimDetailDto
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int HedefId { get; set; }
        public int PageTypeId { get; set; }
        public string Baslik { get; set; } = default!;
        public string? KisaAciklama { get; set; }
        public string? IcerikMetni { get; set; }
        public string? Link { get; set; }
        public string? ResimUrl { get; set; }
        public string? Kategori { get; set; }
        public int Sira { get; set; }
        public int GosterimSayisi { get; set; }
        public DateTime YayimTarihi { get; set; }
        public DateTime EklemeTarihi { get; set; }
        public DateTime? BaslamaTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public string SeoUrl { get; set; } = default!;
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        // Eager loading ile gelen PageType bilgisi
        public PageTypeDto PageType { get; set; } = default!;
    }
}
