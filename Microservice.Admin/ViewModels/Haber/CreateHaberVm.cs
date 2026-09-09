using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.Haber
{
    // Mikroservice.Site.Application.Features.HaberFeatures.CreateHaber.CreateHaberCommandValidation
    // kurallariyla paralel tutulmustur (istemci tarafi on dogrulama).
    public class CreateHaberVm : IValidatableObject
    {
        [Range(1, int.MaxValue, ErrorMessage = "SiteId 0'dan büyük olmalıdır.")]
        public int SiteId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "DilId 0'dan büyük olmalıdır.")]
        public int DilId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Hedef seçimi zorunludur.")]
        public int HedefId { get; set; }

        public int PageTypeId { get; set; }

        [Required(ErrorMessage = "Başlık boş olamaz.")]
        [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
        public string Baslik { get; set; } = default!;

      
        [StringLength(500, ErrorMessage = "Kısa açıklama en fazla 500 karakter olabilir.")]
        public string? KisaAciklama { get; set; } 

        public string? IcerikMetni { get; set; } 

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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (YayimTarihi == default)
                yield return new ValidationResult("Yayım tarihi boş olamaz.", new[] { nameof(YayimTarihi) });

            if (BaslamaTarihi.HasValue && BitisTarihi.HasValue && BaslamaTarihi.Value >= BitisTarihi.Value)
                yield return new ValidationResult("Başlama tarihi, bitiş tarihinden önce olmalıdır.", new[] { nameof(BaslamaTarihi), nameof(BitisTarihi) });

            if (YayimTarihi != default && BitisTarihi.HasValue && YayimTarihi >= BitisTarihi.Value)
                yield return new ValidationResult("Yayım tarihi, bitiş tarihinden önce olmalıdır.", new[] { nameof(YayimTarihi), nameof(BitisTarihi) });
        }
    }
}
