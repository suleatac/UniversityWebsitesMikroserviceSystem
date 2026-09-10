using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.Template
{
    // Mikroservice.Site.Application.Features.TemplateFeatures.UpdateTemplate.UpdateTemplateCommandValidation
    // kurallariyla paralel tutulmustur (istemci tarafi on dogrulama).
    public class UpdateTemplateVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Şablon adı boş olamaz.")]
        [StringLength(200, ErrorMessage = "Şablon adı en fazla 200 karakter olabilir.")]
        public string TemplateAdi { get; set; } = default!;

        [Required(ErrorMessage = "Şablon türü boş olamaz.")]
        public string TemplateTuru { get; set; } = default!;
    }
}
