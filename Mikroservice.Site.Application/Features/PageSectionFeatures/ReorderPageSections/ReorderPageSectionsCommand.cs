using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.PageSectionFeatures.ReorderPageSections
{
    public record ReorderPageSectionsCommand : IRequestByServiceResult
    {
        public List<PageSectionOrderItem> Items { get; init; } = [];
    }

    public record PageSectionOrderItem(int Id, int Sira);
}
