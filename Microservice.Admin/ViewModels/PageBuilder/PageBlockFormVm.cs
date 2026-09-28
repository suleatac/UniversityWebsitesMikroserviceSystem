using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.PageBuilder
{
    /// <summary>
    /// Section ici container (block) olusturma/duzenleme form modeli.
    /// Site API CreatePageBlockCommand / UpdatePageBlockCommand alanlariyla paraleldir.
    /// </summary>
    public class PageBlockFormVm : IValidatableObject
    {
        public int Id { get; set; }

        public int PageSectionId { get; set; }

        [Display(Name = "Üst Satır (Opsiyonel)")]
        public int? ParentId { get; set; }

        [Required(ErrorMessage = "İçerik tipi seçilmelidir.")]
        [Display(Name = "İçerik Tipi")]
        public string ContentType { get; set; } = "text";

        [Display(Name = "İçerik (HTML)")]
        public string? Content { get; set; }

        [Display(Name = "Video URL")]
        [StringLength(500, ErrorMessage = "Video URL en fazla 500 karakter olabilir.")]
        public string? VideoUrl { get; set; }

        [Display(Name = "Video Tipi")]
        public string? VideoType { get; set; } = "YouTube";

        [Display(Name = "Arka Plan / Görsel URL")]
        [StringLength(500, ErrorMessage = "Görsel adresi en fazla 500 karakter olabilir.")]
        public string? BackgroundImageUrl { get; set; }

        [Display(Name = "Arka Plan Rengi")]
        [StringLength(50, ErrorMessage = "Arka plan rengi en fazla 50 karakter olabilir.")]
        public string? BackgroundColor { get; set; }

        [Range(1, 12, ErrorMessage = "Kolon genişliği 1 ile 12 arasında olmalıdır.")]
        [Display(Name = "Kolon Genişliği")]
        public int ColumnSize { get; set; } = 12;

        [Display(Name = "Satır No")]
        public int RowNumber { get; set; }

        [Display(Name = "Animasyon")]
        [StringLength(50, ErrorMessage = "Animasyon adı en fazla 50 karakter olabilir.")]
        public string? Animation { get; set; } = "fadeInUp";

        /// <summary>Carousel tipi icin slaytlar (form tarafinda dinamik satirlarla toplanir).</summary>
        public List<PageBlockMediaInputVm> Medias { get; set; } = [];

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.Equals(ContentType, "text", StringComparison.OrdinalIgnoreCase)
                && string.IsNullOrWhiteSpace(Content))
            {
                yield return new ValidationResult("Metin içeriği boş olamaz.", new[] { nameof(Content) });
            }

            if (string.Equals(ContentType, "video", StringComparison.OrdinalIgnoreCase)
                && string.IsNullOrWhiteSpace(VideoUrl))
            {
                yield return new ValidationResult("Video URL boş olamaz.", new[] { nameof(VideoUrl) });
            }

            if (string.Equals(ContentType, "image", StringComparison.OrdinalIgnoreCase)
                && string.IsNullOrWhiteSpace(BackgroundImageUrl))
            {
                yield return new ValidationResult("Görsel adresi boş olamaz.", new[] { nameof(BackgroundImageUrl) });
            }

            if (string.Equals(ContentType, "carousel", StringComparison.OrdinalIgnoreCase)
                && Medias.Count == 0)
            {
                yield return new ValidationResult("Carousel için en az bir slayt (resim) gereklidir.", new[] { nameof(Medias) });
            }
        }
    }

    public class PageBlockMediaInputVm
    {
        [Display(Name = "Resim URL")]
        public string ResimUrl { get; set; } = default!;

        [Display(Name = "Video URL (Opsiyonel)")]
        public string? VideoUrl { get; set; }

        public int Sira { get; set; }
    }

    /// <summary>Blocks sayfasinda ust block secimi icin sekmeli liste ogesi.</summary>
    public class PageBlockSelectItemVm
    {
        public int Id { get; set; }
        public string Label { get; set; } = default!;
    }
}
