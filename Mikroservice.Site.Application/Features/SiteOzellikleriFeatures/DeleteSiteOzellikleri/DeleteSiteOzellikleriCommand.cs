using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.SiteOzellikleriFeatures.DeleteSiteOzellikleri
{
    public record DeleteSiteOzellikleriCommand(int Id) : IRequestByServiceResult;
}
