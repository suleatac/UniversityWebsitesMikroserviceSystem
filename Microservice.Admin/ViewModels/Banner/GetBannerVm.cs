using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.Banner
{
    public class GetBannerVm
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int? HedefId { get; set; }
        public string Baslik { get; set; } = default!;
        public string KisaAciklama { get; set; } = default!;
        public string? ResimUrl { get; set; }
        public int Sira { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime YayimTarihi { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? BaslamaTarihi { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? BitisTarihi { get; set; }
    }
}