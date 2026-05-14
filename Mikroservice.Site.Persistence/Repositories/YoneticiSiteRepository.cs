using Microservice.Site.Application.Contracts.IRepositories;
using Microservice.Site.Persistence;
using Microservice.Site.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Repositories
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
        public async Task<bool> AnyWithKeycloakUserIdSiteIdYoneticiTipiIdAsync(string keycloakUserId, int siteId, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<YoneticiSite>().AnyAsync(x =>
                x.KeycloakUserId == keycloakUserId &&
                x.SiteId == siteId , cancellationToken);
        }

        public async Task<List<YoneticiSite>> GetByKeycloakUserIdAsync(string keycloakUserId, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<YoneticiSite>()
                .Include(x => x.Site)
                .Where(x => x.KeycloakUserId == keycloakUserId && !x.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<YoneticiSite>> GetAllWithSiteAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<YoneticiSite>()
                .Include(x => x.Site)
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<YoneticiSite?> GetByIdWithSiteAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<YoneticiSite>()
                .Include(x => x.Site)
                .Where(x => x.Id == id && !x.IsDeleted)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
