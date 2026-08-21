using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.SiteDtos;

namespace Mikroservice.Site.Application.Features.PageRouteFeatures.GetBySlug
{
    public record GetBySlugQuery(int SiteId,string Slug) : IRequestByServiceResult<PageRouteDto>;

}
