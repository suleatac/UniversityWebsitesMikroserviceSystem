using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.Menu
{
    // Mikroservice.Site.Application.Features.MenuFeatures (CreateMenu/UpdateMenu) CommandValidation
    // kurallariyla paralel tutulmustur (istemci tarafi on dogrulama).
    public class MenuDetailVm
    {
        public int Id { get; set; }

        public int PageTypeId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir SiteId girilmelidir.")]
        public int SiteId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir DilId girilmelidir.")]
        public int DilId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Hedef seçimi zorunludur.")]
        public int HedefId { get; set; }

        [Required(ErrorMessage = "Menü adı boş olamaz.")]
        [StringLength(200, ErrorMessage = "Menü adı en fazla 200 karakter olabilir.")]
        public string Baslik { get; set; } = default!;

        [StringLength(500, ErrorMessage = "Link en fazla 500 karakter olabilir.")]
        public string? Link { get; set; } = default!;


        [StringLength(2000, ErrorMessage = "İçerik en fazla 2000 karakter olabilir.")]
        public string? IcerikMetni { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Sıra 0 veya daha büyük olmalıdır.")]
        public int Sira { get; set; }

        public bool MegaMenu { get; set; }

        public int? ParentId { get; set; }

        [StringLength(200, ErrorMessage = "SEO URL en fazla 200 karakter olabilir.")]
        public string SeoUrl { get; set; } = default!;

        [StringLength(200, ErrorMessage = "SEO başlık en fazla 200 karakter olabilir.")]
        public string? SeoTitle { get; set; }

        [StringLength(500, ErrorMessage = "SEO açıklama en fazla 500 karakter olabilir.")]
        public string? SeoDescription { get; set; }
    }
}
