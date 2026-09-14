using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.EtkinlikDtos;

namespace Mikroservice.Site.Application.Features.EtkinlikFeatures.GetEtkinliks
{
    public record GetEtkinliksQuery(int SiteId, int DilId) : IRequestByServiceResult<List<EtkinlikDto>>;
}
