using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.GetSikcaSorulanSoruKategori
{
    public record GetSikcaSorulanSoruKategoriQuery : IRequestByServiceResult<List<SikcaSorulanSoruKategori>>;
}
