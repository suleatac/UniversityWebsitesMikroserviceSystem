using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.UnvanFeatures.CreateUnvan
{
    public record CreateUnvanCommand : IRequestByServiceResult<CreateUnvanResponse>
    {
        public int TipId { get; init; }

        public string Ad { get; init; } = default!;

        public string KisaAd { get; init; } = default!;

        public int Sira { get; init; }
        public int? ParentId { get; init; }
    }
}
