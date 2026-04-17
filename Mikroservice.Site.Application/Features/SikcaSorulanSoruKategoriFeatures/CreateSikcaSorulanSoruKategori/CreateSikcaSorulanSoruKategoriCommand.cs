using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.CreateSikcaSorulanSoruKategori
{
    public record CreateSikcaSorulanSoruKategoriCommand : IRequestByServiceResult
    {
        public string Ad { get; init; } = default!;
        public int Sira { get; init; }
        public bool IsDeleted { get; init; } = false;
    }
}
