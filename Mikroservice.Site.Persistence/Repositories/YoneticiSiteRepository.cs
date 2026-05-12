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
        public async Task<bool> AnyWithPersonelIdSiteIdYoneticiTipiIdAsync(int personelId, int siteId, int yoneticiTipiId,CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<YoneticiSite>().AnyAsync(x =>
                x.PersonelId == personelId &&
                x.SiteId == siteId &&
                x.YoneticiTipiId == yoneticiTipiId, cancellationToken);
        }
    }
}
