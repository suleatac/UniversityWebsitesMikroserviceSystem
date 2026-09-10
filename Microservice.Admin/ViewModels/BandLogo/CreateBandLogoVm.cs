using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.BandLogo
{
    // Mikroservice.Site.Application.Features.BandLogoFeatures.CreateBandLogo.CreateBandLogoCommandValidation
    // kurallariyla paralel tutulmustur (istemci tarafi on dogrulama).
    public class CreateBandLogoVm
    {
        [Range(1, int.MaxValue, ErrorMessage = "SiteId 0'dan büyük olmalıdır.")]
        public int SiteId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "DilId 0'dan büyük olmalıdır.")]
        public int DilId { get; set; }

        [Required(ErrorMessage = "Ad boş olamaz.")]
        [StringLength(200, ErrorMessage = "Ad en fazla 200 karakter olabilir.")]
        public string Ad { get; set; } = default!;

        [Required(ErrorMessage = "Görsel URL boş olamaz.")]
        [StringLength(500, ErrorMessage = "Görsel URL en fazla 500 karakter olabilir.")]
        [Url(ErrorMessage = "Geçerli bir URL giriniz.")]
        public string ImgUrl { get; set; } = default!;

        [StringLength(500, ErrorMessage = "Link en fazla 500 karakter olabilir.")]
        [Url(ErrorMessage = "Geçerli bir link giriniz.")]
        public string? Link { get; set; }
    }
}
