using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.BannerFeatures.DeleteBanner
{
    public record DeleteBannerCommand(int Id) : IRequestByServiceResult;
}
