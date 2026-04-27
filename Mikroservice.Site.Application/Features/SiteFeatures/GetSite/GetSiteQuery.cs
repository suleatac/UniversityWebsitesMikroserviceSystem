using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.SiteDtos;

namespace Mikroservice.Site.Application.Features.SiteFeatures.GetSite
{
    public record GetSiteQuery : IRequestByServiceResult<List<SiteDto>>;
}
