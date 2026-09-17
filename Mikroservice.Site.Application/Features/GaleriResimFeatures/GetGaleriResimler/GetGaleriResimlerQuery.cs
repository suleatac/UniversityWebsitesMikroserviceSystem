using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.GaleriResimDtos;

namespace Mikroservice.Site.Application.Features.GaleriResimFeatures.GetGaleriResimler
{
    public record GetGaleriResimlerQuery(int SiteId, int DilId) : IRequestByServiceResult<List<GaleriResimDto>>;
}
