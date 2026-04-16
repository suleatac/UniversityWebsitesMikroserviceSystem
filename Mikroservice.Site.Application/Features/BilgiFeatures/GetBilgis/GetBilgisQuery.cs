using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.BilgiFeatures.GetBilgis
{
    public record GetBilgisQuery(int SiteId, int DilId) : IRequestByServiceResult<List<Bilgi>>;
}
