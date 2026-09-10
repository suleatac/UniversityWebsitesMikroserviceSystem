using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.Template
{
    // Mikroservice.Site.Application.Features.TemplateFeatures.CreateTemplate.CreateTemplateCommandValidation
    // kurallariyla paralel tutulmustur (istemci tarafi on dogrulama).
    public class CreateTemplateVm
    {
        [Required(ErrorMessage = "Şablon adı boş olamaz.")]
        [StringLength(200, ErrorMessage = "Şablon adı en fazla 200 karakter olabilir.")]
        public string TemplateAdi { get; set; } = default!;

        [Required(ErrorMessage = "Şablon türü boş olamaz.")]
        public string TemplateTuru { get; set; } = default!;

    }
}
