using Microservice.Web.ViewModels.Pages;

namespace Microservice.Web.ViewModels.SitePersonel
{
    public class GetPersonelVm
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int PersonelId { get; set; }
        public int UnvanId { get; set; }
        public int PersonelTipId { get; set; }
        public int PageTypeId { get; set; }
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


        public PagesDetailVm PageType { get; set; } = default!;
    }
}
