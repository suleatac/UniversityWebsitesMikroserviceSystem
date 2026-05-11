using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.MenuFeatures.CreateMenu
{
    public record CreateMenuCommand : IRequestByServiceResult<CreateMenuResponse>
    {
        public int SiteId { get; init; }

        public int DilId { get; init; }

        public int HedefId { get; init; }

        public string Ad { get; init; } = default!;

        public string Link { get; init; } = default!;

        public string? IconUrl { get; init; }

        public string? Icerik { get; init; }

        public int Sira { get; init; }

        public bool MegaMenu { get; init; }

        public int? ParentId { get; init; }
    }
}
