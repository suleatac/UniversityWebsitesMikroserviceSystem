using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SiteOzellikleriFeatures.GetSiteOzellikleri
{
    public record GetSiteOzellikleriQuery(int SiteId) : IRequestByServiceResult<SiteOzellikleri>;
}
