using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.BirimFeatures.UpdateBirim
{
    public record UpdateBirimCommand : IRequestByServiceResult
    {
        public int Id { get; init; }
        public int? ParentId { get; init; }

        public string Ad { get; init; } = default!;

        public int Sira { get; init; }
    }
}
