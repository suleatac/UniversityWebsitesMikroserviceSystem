using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.CreateSikcaSorulanSoruKategori
{
    public record CreateSikcaSorulanSoruKategoriCommand : IRequestByServiceResult<CreateSikcaSorulanSoruKategoriResponse>
    {
        public string Ad { get; init; } = default!;
        public int Sira { get; init; }
    }
}
