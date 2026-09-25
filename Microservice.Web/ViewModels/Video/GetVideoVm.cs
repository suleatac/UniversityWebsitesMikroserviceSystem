using Microservice.Web.ViewModels.Pages;

namespace Microservice.Web.ViewModels.Video
{
    public class GetVideoVm
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int HedefId { get; set; }
        public int PageTypeId { get; set; }
        public string Baslik { get; set; } = default!;
        public string KisaAciklama { get; set; } = default!;
        public string? ResimUrl { get; set; }
        public string? VideoUrl { get; set; }

        // Detay linki uretimi icin (API VideoDto karsiligi)
        public string? Link { get; set; }
        public string SeoUrl { get; set; } = default!;
        public DateTime YayimTarihi { get; set; }
        public DateTime? BaslamaTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public PagesDetailVm PageType { get; set; } = default!;
    }
}
