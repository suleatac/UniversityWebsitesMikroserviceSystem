using Mikroservice.Site.Application.DTOs.PageTypeDtos;
using Mikroservice.Site.Domain.Enums;

namespace Mikroservice.Site.Application.DTOs.IcerikDtos
{
    /// <summary>
    /// Icerik (TPH: Haber, Duyuru, Bilgi, Etkinlik, Video) tablosunda
    /// yapilan arama sonucunda donen tek bir icerik kaydi.
    /// </summary>
    public class IcerikSearchDto
    {
        public int Id { get; set; }

        // Icerigin gercek tipi (discriminator). Menu/Banner arama sonuclarinda yer almaz.
        public IcerikTip Tip { get; set; }

        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int PageTypeId { get; set; }

        public string Baslik { get; set; } = default!;
        public string? KisaAciklama { get; set; }
        public string? ResimUrl { get; set; }
        public string? Link { get; set; }
        public string SeoUrl { get; set; } = default!;
        public DateTime YayimTarihi { get; set; }

        // Detay linki icin PageType.Slug needed
        public PageTypeDto PageType { get; set; } = default!;
    }
}
