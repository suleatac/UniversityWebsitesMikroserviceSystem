using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.Popup
{
    // Mikroservice.Site.Application.Features.PopupFeatures.CreatePopup.CreatePopupCommandValidation
    // kurallariyla paralel tutulmustur (istemci tarafi on dogrulama).
    public class CreatePopupVm : IValidatableObject
    {
        [Range(1, int.MaxValue, ErrorMessage = "Sayfa türü seçimi zorunludur.")]
        public int PageTypeId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "SiteId 0'dan büyük olmalıdır.")]
        public int SiteId { get; set; }

        [Required(ErrorMessage = "Başlık boş olamaz.")]
        [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
        public string? Baslik { get; set; } = default!;

        [StringLength(500, ErrorMessage = "Kısa açıklama en fazla 500 karakter olabilir.")]
        public string? KisaAciklama { get; set; } = default!;

        public string? IcerikMetni { get; set; } = default!;

        [StringLength(500, ErrorMessage = "Link en fazla 500 karakter olabilir.")]
        [Url(ErrorMessage = "Geçerli bir link giriniz.")]
        public string? Link { get; set; }

        [StringLength(500, ErrorMessage = "Resim URL en fazla 500 karakter olabilir.")]
        [Url(ErrorMessage = "Geçerli bir resim URL giriniz.")]
        public string? ResimUrl { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime YayimTarihi { get; set; } = DateTime.Now;

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? BaslamaTarihi { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? BitisTarihi { get; set; }

        [StringLength(200, ErrorMessage = "SEO URL en fazla 200 karakter olabilir.")]
        public string? SeoUrl { get; set; }

        [StringLength(200, ErrorMessage = "SEO başlık en fazla 200 karakter olabilir.")]
        public string? SeoTitle { get; set; }

        [StringLength(500, ErrorMessage = "SEO açıklama en fazla 500 karakter olabilir.")]
        public string? SeoDescription { get; set; }

        public bool TamEkranMi { get; set; } = false;

        public int GosterimSuresiSaniye { get; set; }

        public bool CookieIleTekrarGosterme { get; set; } = true;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (YayimTarihi == default)
                yield return new ValidationResult("Yayım tarihi boş olamaz.", new[] { nameof(YayimTarihi) });

            if (BaslamaTarihi.HasValue && BitisTarihi.HasValue && BaslamaTarihi.Value >= BitisTarihi.Value)
                yield return new ValidationResult("Başlama tarihi, bitiş tarihinden önce olmalıdır.", new[] { nameof(BaslamaTarihi), nameof(BitisTarihi) });

            if (YayimTarihi != default && BitisTarihi.HasValue && YayimTarihi >= BitisTarihi.Value)
                yield return new ValidationResult("Yayım tarihi, bitiş tarihinden önce olmalıdır.", new[] { nameof(YayimTarihi), nameof(BitisTarihi) });

            if (!TamEkranMi && GosterimSuresiSaniye <= 0)
                yield return new ValidationResult("Tam ekran değilse, gösterim süresi saniye cinsinden 0'dan büyük olmalıdır.", new[] { nameof(GosterimSuresiSaniye), nameof(TamEkranMi) });

            if (CookieIleTekrarGosterme && GosterimSuresiSaniye <= 0)
                yield return new ValidationResult("Cookie ile tekrar gösterme seçeneği aktifse, gösterim süresi saniye cinsinden 0'dan büyük olmalıdır.", new[] { nameof(GosterimSuresiSaniye), nameof(CookieIleTekrarGosterme) });
        }
    }
}
