using MediatR;
using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.YoneticiSiteDtos;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.GetYoneticiSites
{
    public record GetYoneticiSitesQuery
      : IRequest<ServiceResult<List<YoneticiSiteDetailDto>>>;
}
