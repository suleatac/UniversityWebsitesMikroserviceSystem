using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IYoneticiSiteRepository : IGenericRepository<YoneticiSite>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task<bool> AnyWithKeycloakUserIdSiteIdYoneticiTipiIdAsync(string keycloakUserId, int siteId, CancellationToken cancellationToken = default);
        Task<List<YoneticiSite>> GetByKeycloakUserIdAsync(string keycloakUserId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tüm YoneticiSite kayıtlarını Site navigation property Include ederek getirir.
        /// </summary>
        Task<List<YoneticiSite>> GetAllWithSiteAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Tek bir YoneticiSite kaydını Site navigation property Include ederek getirir.
        /// </summary>
        Task<YoneticiSite?> GetByIdWithSiteAsync(int id, CancellationToken cancellationToken = default);
    }
}
