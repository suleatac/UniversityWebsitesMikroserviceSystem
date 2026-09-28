using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.PageSectionFeatures.UpdatePageSection
{
    public record UpdatePageSectionCommand : IRequestByServiceResult
    {
        public int Id { get; init; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public string Baslik { get; set; } = default!;
        public string? BackgroundColor { get; set; }
        public string? BackgroundImageUrl { get; set; }
        public int Sira { get; set; }
        public bool Yayinda { get; set; } = true;
    }
}
