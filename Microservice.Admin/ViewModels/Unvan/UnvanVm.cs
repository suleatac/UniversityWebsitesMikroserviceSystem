using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.Unvan
{
    // Mikroservice.Site.Application.Features.UnvanFeatures (Create/Update) CommandValidation
    // kurallariyla paralel tutulmustur (istemci tarafi on dogrulama).
    public class UnvanVm
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir TipId girilmelidir.")]
        public int TipId { get; set; }

        [Required(ErrorMessage = "Ad boş olamaz.")]
        [StringLength(200, ErrorMessage = "Ad en fazla 200 karakter olabilir.")]
        public string Ad { get; set; } = default!;

        [Required(ErrorMessage = "Kısa ad boş olamaz.")]
        [StringLength(50, ErrorMessage = "Kısa ad en fazla 50 karakter olabilir.")]
        public string KisaAd { get; set; } = default!;

        public int Sira { get; set; }

        public int? ParentId { get; set; }
    }
}
