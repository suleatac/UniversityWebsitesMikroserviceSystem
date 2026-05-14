using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.Popup
{
    public class CreatePopupVm
    {
        public int SiteId { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur")]
        public string Baslik { get; set; } = default!;
        [Required(ErrorMessage = "Kısa Açıklama zorunludur")]
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

        public string? SeoUrl { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }

        public bool TamEkranMi { get; set; } = false;
        public int GosterimSuresiSaniye { get; set; }
        public bool CookieIleTekrarGosterme { get; set; } = true;
    }
}