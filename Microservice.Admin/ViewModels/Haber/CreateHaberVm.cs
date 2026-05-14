using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.Haber
{
    public class CreateHaberVm
    {
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int? HedefId { get; set; }

        public string Baslik { get; set; } = default!;
        public string KisaAciklama { get; set; } = default!;
        public string IcerikMetni { get; set; } = default!;

        public string? Link { get; set; }
        public string? ResimUrl { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime YayimTarihi { get; set; } = DateTime.Now;
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? BaslamaTarihi { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? BitisTarihi { get; set; }

        // SEO (opsiyonel ama önemli)
        public string? SeoUrl { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
    }
}
