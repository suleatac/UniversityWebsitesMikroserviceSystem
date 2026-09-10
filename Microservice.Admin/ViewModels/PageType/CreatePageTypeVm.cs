using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.PageType
{
    // Mikroservice.Site.Application.Features.PageTypeFeatures (CreatePageType/UpdatePageType)
    // CommandValidation kurallariyla paralel tutulmustur.
    public class CreatePageTypeVm
    {
        [Range(1, int.MaxValue, ErrorMessage = "PageType seçimi zorunludur.")]
        public int PageTypeKind { get; set; }

        [Required(ErrorMessage = "Ad boş olamaz.")]
        [StringLength(200, ErrorMessage = "Ad en fazla 200 karakter olabilir.")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Slug boş olamaz.")]
        [StringLength(200, ErrorMessage = "Slug en fazla 200 karakter olabilir.")]
        public string Slug { get; set; } = default!;

        [Range(1, int.MaxValue, ErrorMessage = "Template seçimi zorunludur.")]
        public int TemplateId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Dil seçimi zorunludur.")]
        public int DilId { get; set; }

        [StringLength(200, ErrorMessage = "ViewName en fazla 200 karakter olabilir.")]
        public string? ViewName { get; set; }

        public bool IsHomePage { get; set; }
        public bool IsActive { get; set; } = true;
    }
}