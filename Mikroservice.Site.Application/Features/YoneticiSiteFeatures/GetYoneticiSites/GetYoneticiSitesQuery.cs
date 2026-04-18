using MediatR;
using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.GetYoneticiSites
{
    public record GetYoneticiSitesQuery(int SiteId)
      : IRequest<ServiceResult<List<YoneticiSite>>>;
}
