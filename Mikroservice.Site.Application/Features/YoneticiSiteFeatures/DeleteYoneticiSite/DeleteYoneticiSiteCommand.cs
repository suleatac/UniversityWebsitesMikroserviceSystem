using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.DeleteYoneticiSite
{
    public record DeleteYoneticiSiteCommand(int Id) : IRequestByServiceResult;
}
