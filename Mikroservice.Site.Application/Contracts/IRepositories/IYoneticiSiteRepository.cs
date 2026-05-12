using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IYoneticiSiteRepository : IGenericRepository<YoneticiSite>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task<bool> AnyWithKeycloakUserIdSiteIdYoneticiTipiIdAsync(string keycloakUserId, int siteId, int yoneticiTipiId, CancellationToken cancellationToken = default);
        Task<List<YoneticiSite>> GetByKeycloakUserIdAsync(string keycloakUserId, CancellationToken cancellationToken = default);
    }
}
