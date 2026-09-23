using Mikroservice.Site.Application.DTOs.PageTypeDtos;

namespace Mikroservice.Site.Application.DTOs.EtkinlikDtos
{
    public class EtkinlikDto
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int HedefId { get; set; }
        public int PageTypeId { get; set; }
        public string? Baslik { get; set; }
        public string? KisaAciklama { get; set; } 
        public string ResimUrl { get; set; } = default!;
        public DateTime YayimTarihi { get; set; }
        public DateTime? BaslamaTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public string SeoUrl { get; set; } = default!;
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public PageTypeDto PageType { get; set; } = default!;
    }
}