using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.YoneticiSiteDtos;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.GetYoneticiSiteById
{
    public record GetYoneticiSiteByIdQuery(int Id) : IRequestByServiceResult<YoneticiSiteDetailDto>;
}