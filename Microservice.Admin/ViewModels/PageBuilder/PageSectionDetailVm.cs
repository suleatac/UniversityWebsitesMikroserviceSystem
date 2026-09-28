using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.PageBuilder
{
    /// <summary>
    /// Bolum olusturma/duzenleme form modeli.
    /// Site API CreatePageSectionCommand / UpdatePageSectionCommand alanlariyla paraleldir.
    /// </summary>
    public class PageSectionDetailVm
    {
        public int Id { get; set; }

        public int SiteId { get; set; }

        public int DilId { get; set; }

        [Required(ErrorMessage = "Bölüm başlığı boş olamaz.")]
        [StringLength(200, ErrorMessage = "Bölüm başlığı en fazla 200 karakter olabilir.")]
        public string Baslik { get; set; } = default!;

        [Display(Name = "Arka Plan Rengi")]
        [StringLength(50, ErrorMessage = "Arka plan rengi en fazla 50 karakter olabilir.")]
        public string? BackgroundColor { get; set; }

        [Display(Name = "Arka Plan Görseli")]
        [StringLength(500, ErrorMessage = "Arka plan görseli en fazla 500 karakter olabilir.")]
        public string? BackgroundImageUrl { get; set; }

        [Display(Name = "Sıra")]
        public int Sira { get; set; }

        [Display(Name = "Yayında")]
        public bool Yayinda { get; set; } = true;
    }
}
