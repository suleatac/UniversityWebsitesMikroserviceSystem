using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.BannerFeatures.ReorderBanners
{
    public sealed record ReorderBannerItem(int Id, int Sira);

    public sealed record ReorderBannersCommand(
        List<ReorderBannerItem> Items
    ) : IRequestByServiceResult;
}
