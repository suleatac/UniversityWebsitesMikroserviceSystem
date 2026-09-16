using Microservice.Web.ViewModels.Pages;

namespace Microservice.Web.ViewModels.Icerik
{
    /// <summary>
    /// Site API'deki IcerikSearchDto karsiligi.
    /// Tip: 1=Haber, 2=Duyuru, 3=Bilgi, 4=Etkinlik, 5=Video
    /// </summary>
    public class IcerikSearchVm
    {
        public int Id { get; set; }
        public int Tip { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int PageTypeId { get; set; }
        public string Baslik { get; set; } = default!;
        public string? KisaAciklama { get; set; }
        public string? ResimUrl { get; set; }
        public string? Link { get; set; }
        public string SeoUrl { get; set; } = default!;
        public DateTime YayimTarihi { get; set; }
        public PagesDetailVm PageType { get; set; } = default!;
    }
}
