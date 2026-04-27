using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.SiteDtos;

namespace Mikroservice.Site.Application.Features.SiteFeatures.GetSiteById
{
    public record GetSiteByIdQuery(int Id) : IRequestByServiceResult<SiteDetailDto>;
}
