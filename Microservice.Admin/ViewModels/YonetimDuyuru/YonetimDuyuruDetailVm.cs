using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.YonetimDuyuru
{
    // Mikroservice.Site.Application.Features.YonetimDuyuruFeatures.UpdateYonetimDuyuru.UpdateYonetimDuyuruCommandValidation
    // kurallariyla paralel tutulmustur (istemci tarafi on dogrulama).
    public class YonetimDuyuruDetailVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık boş olamaz.")]
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir.")]
        public string Baslik { get; set; } = default!;

        public string Icerik { get; set; } = default!;

        public DateTime EklenmeTarihi { get; set; }

        public bool OkunduMu { get; set; }
    }
}
