using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.SikcaSorulanSoru
{
    // Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.CreateSikcaSorulanSoru.CreateSikcaSorulanSoruCommandValidation
    // kurallariyla paralel tutulmustur (istemci tarafi on dogrulama).
    public class CreateSikcaSorulanSoruVm
    {
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir SiteId girilmelidir.")]
        public int SiteId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir DilId girilmelidir.")]
        public int DilId { get; set; }

        public int? ParentId { get; set; }

        [Required(ErrorMessage = "Soru boş olamaz.")]
        [StringLength(500, ErrorMessage = "Soru en fazla 500 karakter olabilir.")]
        public string Soru { get; set; } = default!;

        [Required(ErrorMessage = "Cevap boş olamaz.")]
        public string Cevap { get; set; } = default!;

        [Range(0, int.MaxValue, ErrorMessage = "Sıra 0 veya daha büyük olmalıdır.")]
        public int Sira { get; set; } = 0;


    }
}
