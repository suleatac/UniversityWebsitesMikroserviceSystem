using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.EtkinlikDtos;

namespace Mikroservice.Site.Application.Features.EtkinlikFeatures.GetEtkinlikBySeoUrl
{
    public record GetEtkinlikBySeoUrlQuery(int SiteId, int DilId, string SeoUrl) : IRequestByServiceResult<EtkinlikDetailDto>;
}
