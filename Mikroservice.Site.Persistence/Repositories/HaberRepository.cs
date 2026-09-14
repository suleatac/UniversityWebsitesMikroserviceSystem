using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Persistence.Repositories
{
    public class HaberRepository : GenericRepository<Haber>, IHaberRepository
    {
        private readonly AppDbContext _appDbContext;

        public HaberRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<Haber>().AnyAsync(cancellationToken);
        }

        public Task<Haber?> GetBySeoUrlAsync(int siteId, int dilId, string seoUrl, CancellationToken cancellationToken = default)
        {
            return _appDbContext.Set<Haber>()
                .AsNoTracking()
                .Include(x => x.PageType) // eager loading
                .FirstOrDefaultAsync(x => x.SiteId == siteId && x.DilId == dilId && x.SeoUrl == seoUrl, cancellationToken);
        }
        public async Task<Haber?> GetByIdWithPageTypeAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<Haber>()
                .AsNoTracking()
                .Include(x => x.PageType) // eager loading
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public async Task<List<Haber>> GetBySiteAndLanguageAsync(int siteId, int dilId, CancellationToken cancellationToken)
        {
            return await _appDbContext
                .Set<Haber>()
                .AsNoTracking()
                .Include(x => x.PageType) // eager loading
                .Where(x => x.SiteId == siteId && x.DilId == dilId).OrderByDescending(x => x.YayimTarihi)
                .ToListAsync(cancellationToken);
        }
    }
}
