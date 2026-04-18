using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.GetSikcaSorulanSoru
{
    public record GetSikcaSorulanSoruQuery(int SiteId, int DilId) : IRequestByServiceResult<List<SikcaSorulanSoru>>;
}

