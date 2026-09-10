using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.Banner
{
    public class CreateBannerVm
    {

        public int PageTypeId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "SiteId 0'dan büyük olmalıdır.")]
        public int SiteId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "DilId 0'dan büyük olmalıdır.")]
        public int DilId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Hedef seçimi zorunludur.")]
        public int HedefId { get; set; }

        [Required(ErrorMessage = "Başlık boş olamaz.")]
        [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
        public string Baslik { get; set; } = default!;


        [StringLength(500, ErrorMessage = "Kısa açıklama en fazla 500 karakter olabilir.")]
        public string? KisaAciklama { get; set; }
        public string? IcerikMetni { get; set; }

        [StringLength(500, ErrorMessage = "Link en fazla 500 karakter olabilir.")]
        [Url(ErrorMessage = "Geçerli bir link giriniz.")]
        public string? Link { get; set; }

        [Required(ErrorMessage = "Banner için resim URL'si gereklidir.")]
        public string ResimUrl { get; set; }= default!;

        public int Sira { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime YayimTarihi { get; set; } = DateTime.Now;
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? BaslamaTarihi { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? BitisTarihi { get; set; }
        [StringLength(200, ErrorMessage = "SEO URL en fazla 200 karakter olabilir.")]
        public string SeoUrl { get; set; } = default!;

        [StringLength(200, ErrorMessage = "SEO başlık en fazla 200 karakter olabilir.")]
        public string? SeoTitle { get; set; }

        [StringLength(500, ErrorMessage = "SEO açıklama en fazla 500 karakter olabilir.")]
        public string? SeoDescription { get; set; }
    }
}