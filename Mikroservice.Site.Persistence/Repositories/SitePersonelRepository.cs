using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Persistence.Repositories
{
    public class SitePersonelRepository : GenericRepository<SitePersonel>, ISitePersonelRepository
    {
        private readonly AppDbContext _appDbContext;

        public SitePersonelRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<SitePersonel>().AnyAsync(cancellationToken);
        }
        public async Task<bool> AnyByUnvanIdAsync(int unvanId,CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<SitePersonel>().AnyAsync(x=>x.UnvanId==unvanId,cancellationToken);
        }
        public async Task<List<SitePersonel>> GetAllWithPersonelTipAndUnvanAsync(int siteId,CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<SitePersonel>()
                .Include(x => x.Unvan).Include(x => x.PersonelTip)
                .Where(x => x.SiteId == siteId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
