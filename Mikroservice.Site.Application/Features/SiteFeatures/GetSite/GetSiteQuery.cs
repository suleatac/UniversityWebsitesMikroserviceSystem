using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.SiteFeatures.GetSite
{
    public record GetSiteQuery(int Id) : IRequestByServiceResult;
}
