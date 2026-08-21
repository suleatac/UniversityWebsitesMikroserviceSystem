using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.TemplateFeatures.CreateTemplate
{
    public record CreateTemplateCommand : IRequestByServiceResult<CreateTemplateResponse>
    {
        public string TemplateAdi { get; set; } = default!;
        public string TemplateTuru { get; set; } = default!;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = default!;

    }
}
