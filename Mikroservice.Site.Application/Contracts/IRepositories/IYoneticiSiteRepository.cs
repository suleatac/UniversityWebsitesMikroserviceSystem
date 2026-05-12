using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IYoneticiSiteRepository : IGenericRepository<YoneticiSite>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task<bool> AnyWithPersonelIdSiteIdYoneticiTipiIdAsync(int personelId, int siteId, int yoneticiTipiId, CancellationToken cancellationToken = default);
    }
}
