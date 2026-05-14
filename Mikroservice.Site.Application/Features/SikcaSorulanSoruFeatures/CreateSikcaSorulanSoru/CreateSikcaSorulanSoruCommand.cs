using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.CreateSikcaSorulanSoru
{
    public record CreateSikcaSorulanSoruCommand : IRequestByServiceResult<CreateSikcaSorulanSoruResponse>
    {
        public int SiteId { get; init; }

        public int DilId { get; init; }
        public int? ParentId { get; init; }
        public string Soru { get; init; } = default!;

        public string Cevap { get; init; } = default!;

        public int Sira { get; init; }

        public string? SeoUrl { get; init; }
    
    }
}
