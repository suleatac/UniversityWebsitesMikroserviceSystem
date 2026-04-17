using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.DeleteSikcaSorulanSoruKategori
{
    public record DeleteSikcaSorulanSoruKategoriCommand(int Id) : IRequestByServiceResult;
}
