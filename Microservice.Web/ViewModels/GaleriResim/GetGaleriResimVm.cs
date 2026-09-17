using Microservice.Web.ViewModels.Pages;
using System.ComponentModel.DataAnnotations;

namespace Microservice.Web.ViewModels.GaleriResim
{
    public class GetGaleriResimVm
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int HedefId { get; set; }
        public int PageTypeId { get; set; }
        public string Baslik { get; set; } = default!;
        public string SeoUrl { get; set; } = default!;
        public string? Link { get; set; }
        public string? KisaAciklama { get; set; }
        public string? ResimUrl { get; set; }
        public string? Kategori { get; set; }
        public int Sira { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime YayimTarihi { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? BaslamaTarihi { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? BitisTarihi { get; set; }
        public PagesDetailVm PageType { get; set; } = default!;
    }
}
