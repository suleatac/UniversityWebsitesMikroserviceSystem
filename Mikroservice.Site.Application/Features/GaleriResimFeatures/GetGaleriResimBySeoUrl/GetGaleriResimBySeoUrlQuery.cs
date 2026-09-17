using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.GaleriResimDtos;

namespace Mikroservice.Site.Application.Features.GaleriResimFeatures.GetGaleriResimBySeoUrl
{
    public record GetGaleriResimBySeoUrlQuery(int SiteId, int DilId, string SeoUrl) : IRequestByServiceResult<GaleriResimDetailDto>;
}
