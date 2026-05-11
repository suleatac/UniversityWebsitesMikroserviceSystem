using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.TemplateFeatures.UpdateTemplate
{
    public record UpdateTemplateCommand : IRequestByServiceResult
    {
        public int Id { get; init; }
        public string TemplateAdi { get; init; } = default!;
        public string TemplateTuru { get; init; } = default!;
        public string? FolderName { get; init; }
        public string? LayoutPath { get; init; }
    }
}
