using Mikroservice.Site.Application.DTOs.PageTypeDtos;

namespace Mikroservice.Site.Application.DTOs.SitePersonelDtos
{
    public class SitePersonelDto
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int PersonelId { get; set; }
        public int UnvanId { get; set; }
        public int PageTypeId { get; set; }
        public int PersonelTipId { get; set; }

        public string? Adi { get; set; }
        public string? Soyadi { get; set; }
        public string? Username { get; set; }


        public string? ResimUrl { get; set; }
        public string? Hakkinda { get; set; }
        public string? UnvanAd { get; set; }
        public string? PersonelTipAd { get; set; }

        // SEO
        public string SeoUrl { get; set; } = default!;
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }


        public PageTypeDto PageType { get; set; } = default!;
    }
}
