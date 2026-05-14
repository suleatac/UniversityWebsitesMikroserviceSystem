using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.Video
{
    public class VideoDetailVm
    {
        public int Id { get; set; }

        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int? HedefId { get; set; }

        public string Baslik { get; set; } = default!;
        public string KisaAciklama { get; set; } = default!;
        public string IcerikMetni { get; set; } = default!;

        public string? Link { get; set; }
        public string? ResimUrl { get; set; }
        public int GosterimSayisi { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime YayimTarihi { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EklemeTarihi { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? BaslamaTarihi { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? BitisTarihi { get; set; }

        public string? SeoUrl { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }

        public string? VideoUrl { get; set; }
    }
}