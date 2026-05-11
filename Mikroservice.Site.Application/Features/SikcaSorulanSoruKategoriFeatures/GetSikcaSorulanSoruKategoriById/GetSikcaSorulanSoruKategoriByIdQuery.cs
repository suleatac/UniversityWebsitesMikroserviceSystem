using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.GetSikcaSorulanSoruKategoriById
{
    public record GetSikcaSorulanSoruKategoriByIdQuery(int Id) : IRequestByServiceResult<SikcaSorulanSoruKategori>;
}