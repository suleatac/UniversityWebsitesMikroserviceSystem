using Microservice.Shared;
using Mikroservice.Site.Domain.Enums;

namespace Microservice.Site.Application.Features.PageTypeFeatures.UpdatePageType
{
    public record UpdatePageTypeCommand : IRequestByServiceResult
    {
        public int Id { get; init; }
        public PageType PageTypeId { get; init; }
        public int SiteId { get; init; }
        public string Name { get; init; } = default!;
        public string Slug { get; init; } = default!;
        public int TemplateId { get; init; }
        public int DilId { get; init; }
        public string? ViewName { get; init; }
        public bool IsHomePage { get; init; }
        public bool IsActive { get; init; }
    }
}