using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.EtkinlikFeatures.GetEtkinliks
{
    public record GetEtkinliksQuery(int SiteId, int DilId) : IRequestByServiceResult<List<Etkinlik>>;
}
