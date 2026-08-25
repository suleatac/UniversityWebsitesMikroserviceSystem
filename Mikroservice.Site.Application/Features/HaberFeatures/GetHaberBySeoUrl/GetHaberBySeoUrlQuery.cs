using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.HaberDtos;

namespace Mikroservice.Site.Application.Features.HaberFeatures.GetHaberBySeoUrl
{
    public record GetHaberBySeoUrlQuery(int SiteId, int DilId, string SeoUrl) : IRequestByServiceResult<HaberDetailDto>;
}