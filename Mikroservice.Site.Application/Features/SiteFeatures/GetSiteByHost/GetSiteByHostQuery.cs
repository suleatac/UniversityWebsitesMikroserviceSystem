using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.SiteDtos;

namespace Mikroservice.Site.Application.Features.SiteFeatures.GetSiteByHost
{
    public record GetSiteByHostQuery(string Host) : IRequestByServiceResult<SiteDetailDto>;
}