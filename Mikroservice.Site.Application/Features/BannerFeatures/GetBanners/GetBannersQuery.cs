using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.BannerFeatures.GetBanners
{
    public record GetBannersQuery(int SiteId, int DilId) : IRequestByServiceResult<List<Banner>>;
}
