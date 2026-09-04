using Microservice.Shared;
using Mikroservice.Site.Domain.Enums;

namespace Mikroservice.Site.Application.Features.PageTypeFeatures.CreatePageType
{
    public record CreatePageTypeCommand : IRequestByServiceResult<CreatePageTypeResponse>
    {
        public PageTypeKind PageTypeKind { get; init; }
        public string Name { get; init; } = default!;
        public string Slug { get; init; } = default!;
        public int DilId { get; init; }
        public int TemplateId { get; init; }
        public string? ViewName { get; init; }
        public bool IsHomePage { get; init; }
    }
}