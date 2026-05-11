using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.UnvanFeatures.UpdateUnvan
{
    public record UpdateUnvanCommand : IRequestByServiceResult
    {
        public int Id { get; set; }
        public int TipId { get; init; }

        public string Ad { get; init; } = default!;

        public string KisaAd { get; init; } = default!;

        public int Sira { get; init; }
        public int? ParentId { get; init; }
    }
}
