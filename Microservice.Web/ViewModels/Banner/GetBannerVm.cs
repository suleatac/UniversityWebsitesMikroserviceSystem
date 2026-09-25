using Microservice.Web.ViewModels.Pages;

namespace Microservice.Web.ViewModels.Banner
{
    public class GetBannerVm
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int HedefId { get; set; }
        public int PageTypeId { get; set; }
        public string Baslik { get; set; } = default!;
        public string KisaAciklama { get; set; } = default!;
        public string ResimUrl { get; set; } = default!;
        public string? Link { get; init; }
        public int Sira { get; set; }
        public DateTime YayimTarihi { get; set; }
        public DateTime? BaslamaTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }

        public string SeoUrl { get; set; } = default!;
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }

        public PagesDetailVm PageType { get; set; } = default!;
    }
}
