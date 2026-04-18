using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.UpdateSikcaSorulanSoruKategori
{
    public record UpdateSikcaSorulanSoruKategoriCommand : IRequestByServiceResult
    {
        public int Id { get; init; }
        public string Ad { get; init; } = default!;
        public int Sira { get; init; }
    }
}
