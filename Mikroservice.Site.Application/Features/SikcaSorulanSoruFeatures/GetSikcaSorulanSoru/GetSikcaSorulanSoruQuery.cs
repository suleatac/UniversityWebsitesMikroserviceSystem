using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.SikcaSorulanSoruDtos;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.GetSikcaSorulanSoru
{
    public record GetSikcaSorulanSoruQuery(int SiteId, int DilId) : IRequestByServiceResult<List<SikcaSorulanSoruDto>>;
}

