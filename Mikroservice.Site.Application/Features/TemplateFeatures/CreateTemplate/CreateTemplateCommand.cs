using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.TemplateFeatures.CreateTemplate
{
    public record CreateTemplateCommand : IRequestByServiceResult
    {
        public string TemplateAdi { get; set; } = default!;
        public string TemplateTuru { get; set; } = default!;
        public string? FolderName { get; set; } = default!;
        public string? LayoutPath { get; set; } = default!;
    }
}
