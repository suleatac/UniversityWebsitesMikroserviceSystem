using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.BilgiDtos;

namespace Mikroservice.Site.Application.Features.BilgiFeatures.GetBilgis
{
    public record GetBilgisQuery(int SiteId, int DilId) : IRequestByServiceResult<List<BilgiDto>>;
}
