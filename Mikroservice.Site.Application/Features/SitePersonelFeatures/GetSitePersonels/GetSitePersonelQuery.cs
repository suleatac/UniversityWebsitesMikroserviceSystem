using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SitePersonelFeatures.GetSitePersonels
{
    public record GetSitePersonelQuery(int SiteId) : IRequestByServiceResult<List<SitePersonel>>;

}
