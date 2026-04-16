using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.BirimFeatures.CreateBirim
{
    public record CreateBirimCommand : IRequestByServiceResult
    {
        public int? ParentId { get; init; }

        public string Ad { get; init; } = default!;

        public int Sira { get; init; }
    }
}
