using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.SitePersonelFeatures.DeleteSitePersonel
{
    public record DeleteSitePersonelCommand(int Id) : IRequestByServiceResult;
}
