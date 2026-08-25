using Microservice.Shared;
using Mikroservice.Site.Domain.Enums;

namespace Mikroservice.Site.Application.Features.PageTypeFeatures.CreatePageType
{
    public record CreatePageTypeCommand : IRequestByServiceResult<CreatePageTypeResponse>
    {
        public int SiteId { get; init; }
        public PageType PageTypeId { get; init; }
        public string Name { get; init; } = default!;
        public string Slug { get; init; } = default!;
        public int DilId { get; init; }
        public int TemplateId { get; init; }
        public string? ViewName { get; init; }
        public bool IsHomePage { get; init; }
        public bool IsActive { get; init; } = true;
    }
}