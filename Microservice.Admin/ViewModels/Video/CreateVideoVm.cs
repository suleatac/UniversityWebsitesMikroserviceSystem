using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.Video
{
    // Mikroservice.Site.Application.Features.VideoFeatures.CreateVideo.CreateVideoCommandValidation
    // kurallariyla paralel tutulmustur (istemci tarafi on dogrulama).
    public class CreateVideoVm : IValidatableObject
    {

        public int PageTypeId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "SiteId 0'dan büyük olmalıdır.")]
        public int SiteId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "DilId 0'dan büyük olmalıdır.")]
        public int DilId { get; set; }

        public int? HedefId { get; set; }

        [Required(ErrorMessage = "Başlık boş olamaz.")]
        [StringLength(300, ErrorMessage = "Başlık en fazla 300 karakter olabilir.")]
        public string Baslik { get; set; } = default!;

        public string? KisaAciklama { get; set; }
        public string? IcerikMetni { get; set; }

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

        [Required(ErrorMessage = "Geçerli video URL giriniz.")]
        [Url(ErrorMessage = "Geçerli video URL giriniz.")]
        public string VideoUrl { get; set; } = default!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (YayimTarihi > DateTime.Now.AddYears(1))
                yield return new ValidationResult("Yayım tarihi bir yıldan fazla ileri bir tarih olamaz.", new[] { nameof(YayimTarihi) });
        }
    }
}
