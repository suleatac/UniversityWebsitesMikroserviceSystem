using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Persistence.Repositories
{
    public class YoneticiSiteRepository : GenericRepository<YoneticiSite>, IYoneticiSiteRepository
    {
        private readonly AppDbContext _appDbContext;

        public YoneticiSiteRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<YoneticiSite>().AnyAsync(cancellationToken);
        }
        public async Task<bool> AnyWithKeycloakUserIdSiteIdYoneticiTipiIdAsync(string keycloakUserId, int siteId, int yoneticiTipiId, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<YoneticiSite>().AnyAsync(x =>
                x.KeycloakUserId == keycloakUserId &&
                x.SiteId == siteId &&
                x.YoneticiTipiId == yoneticiTipiId, cancellationToken);
        }

        public async Task<List<YoneticiSite>> GetByKeycloakUserIdAsync(string keycloakUserId, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<YoneticiSite>()
                .Include(x => x.Site)
                .Include(x => x.YoneticiTipi)
                .Where(x => x.KeycloakUserId == keycloakUserId && !x.IsDeleted)
                .ToListAsync(cancellationToken);
        }
    }
}
