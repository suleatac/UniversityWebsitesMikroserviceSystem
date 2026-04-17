using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.SiteFeatures.DeleteSite
{
    public record DeleteSiteCommand(int Id) : IRequestByServiceResult;
}
