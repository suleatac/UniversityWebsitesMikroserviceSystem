using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.MenuFeatures.CreateMenu
{
    public record CreateMenuCommand : IRequestByServiceResult<CreateMenuResponse>
    {
        public int SiteId { get; init; }
        public int PageTypeId { get; init; }

        public int DilId { get; init; }

        public int HedefId { get; init; }

        public string Baslik { get; init; } = default!;

        public string? Link { get; init; } 

        public string? IconUrl { get; init; }

        public string? IcerikMetni { get; init; }

        public int Sira { get; init; }

        public bool MegaMenu { get; init; }

        public int Location { get; init; } = 1;

        public bool IsVisible { get; init; } = true;

        public int? ParentId { get; init; }

        // SEO (opsiyonel ama önemli)
        public string SeoUrl { get; init; } = default!;
        public string? SeoTitle { get; init; }
        public string? SeoDescription { get; init; }
    }
}
