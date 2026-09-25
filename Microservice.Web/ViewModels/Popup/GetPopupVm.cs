using System.ComponentModel.DataAnnotations;

namespace Microservice.Web.ViewModels.Popup
{
    /// <summary>
    /// Ana sayfada modal olarak gösterilen site popup'i.
    /// Site API'deki Popup entity'sinin web tarafinda kullanilan alanlari.
    /// </summary>
    public class GetPopupVm
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public string? Baslik { get; set; }
        public string? KisaAciklama { get; set; }
        public string? Link { get; set; }
        public string? ResimUrl { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime YayimTarihi { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? BaslamaTarihi { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? BitisTarihi { get; set; }

        public bool TamEkranMi { get; set; }
        public int GosterimSuresiSaniye { get; set; }
        public bool CookieIleTekrarGosterme { get; set; } = true;
    }
}
