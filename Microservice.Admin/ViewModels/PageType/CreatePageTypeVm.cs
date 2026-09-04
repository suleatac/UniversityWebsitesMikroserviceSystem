using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.PageType
{
    public class CreatePageTypeVm
    {
        [Range(1, int.MaxValue, ErrorMessage = "PageType seçimi zorunludur.")]
        public int PageTypeKind { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; } = default!;

        [Required, StringLength(200)]
        public string Slug { get; set; } = default!;

        [Range(1, int.MaxValue, ErrorMessage = "Template seçimi zorunludur.")]
        public int TemplateId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Dil seçimi zorunludur.")]
        public int DilId { get; set; }

        [StringLength(200)]
        public string? ViewName { get; set; }

        public bool IsHomePage { get; set; }
        public bool IsActive { get; set; } = true;
    }
}