using System.ComponentModel.DataAnnotations;

namespace Microservice.Web.ViewModels.Iletisim
{
    /// <summary>
    /// Mikroservice.Site.Application Iletisim GonderIletisimMesajiCommand dogrulama
    /// kurallariyla paralel (istemci tarafi on dogrulama).
    /// </summary>
    public class IletisimMesajiVm
    {
        [Range(1, int.MaxValue, ErrorMessage = "Gecersiz site.")]
        public int SiteId { get; set; }

        [Required(ErrorMessage = "Ad Soyad boş olamaz.")]
        [StringLength(150, ErrorMessage = "Ad Soyad en fazla 150 karakter olabilir.")]
        public string AdSoyad { get; set; } = default!;

        [Required(ErrorMessage = "E-posta boş olamaz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [StringLength(200, ErrorMessage = "E-posta en fazla 200 karakter olabilir.")]
        public string Eposta { get; set; } = default!;

        [Required(ErrorMessage = "Konu boş olamaz.")]
        [StringLength(300, ErrorMessage = "Konu en fazla 300 karakter olabilir.")]
        public string Konu { get; set; } = default!;

        [Required(ErrorMessage = "Mesaj boş olamaz.")]
        [StringLength(5000, ErrorMessage = "Mesaj en fazla 5000 karakter olabilir.")]
        public string Mesaj { get; set; } = default!;

        [StringLength(50, ErrorMessage = "Telefon en fazla 50 karakter olabilir.")]
        public string? Telefon { get; set; }
    }
}
