using System.ComponentModel.DataAnnotations;

namespace Microservice.Web.ViewModels.ShortcutButton
{
    public class ShortcutButtonVm
    {
        public int Id { get; set; }

        public int SiteId { get; set; }

        public int DilId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Hedef seçimi zorunludur.")]
        public int HedefId { get; set; }

        [Required(ErrorMessage = "Kısayol buton adı boş olamaz.")]
        [StringLength(200, ErrorMessage = "Kısayol buton adı en fazla 200 karakter olabilir.")]
        [Display(Name = "Buton Adı")]
        public string Ad { get; set; } = default!;
        [StringLength(500, ErrorMessage = "Kısa açıklama en fazla 500 karakter olabilir.")]
        [Display(Name = "Kısa Açıklama")]
        public string? KisaAciklama { get; set; }
        [StringLength(500, ErrorMessage = "Link en fazla 500 karakter olabilir.")]
        public string? Link { get; set; }

        [StringLength(500, ErrorMessage = "IconUrl en fazla 500 karakter olabilir.")]
        [Display(Name = "İkon")]
        public string? IconUrl { get; set; }

        [StringLength(500, ErrorMessage = "ImageUrl en fazla 500 karakter olabilir.")]
        [Display(Name = "Resim")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Görsel Türü")]
        public bool? IsIconImage { get; set; }

        public int Sira { get; set; }
    }
}
