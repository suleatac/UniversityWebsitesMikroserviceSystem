using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Persistence.Repositories
{
    public class EtkinlikRepository : GenericRepository<Etkinlik>, IEtkinlikRepository
    {
        private readonly AppDbContext _appDbContext;

        public EtkinlikRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<Etkinlik>().AnyAsync(cancellationToken);
        }
        public async Task<Etkinlik?> GetByIdWithPageTypeAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<Etkinlik>()
                .AsNoTracking()
                .Include(x => x.PageType) // eager loading
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public Task<Etkinlik?> GetBySeoUrlAsync(int siteId, int dilId, string seoUrl, CancellationToken cancellationToken = default)
        {
            return _appDbContext.Set<Etkinlik>()
                .AsNoTracking()
                .Include(x => x.PageType) // eager loading
                .FirstOrDefaultAsync(x => x.SiteId == siteId && x.DilId == dilId && x.SeoUrl == seoUrl, cancellationToken);
        }
        public async Task<List<Etkinlik>> GetBySiteAndLanguageAsync(int siteId, int dilId, CancellationToken cancellationToken)
        {
            return await _appDbContext
                .Set<Etkinlik>()
                .AsNoTracking()
                .Include(x => x.PageType) // eager loading
                .Where(x => x.SiteId == siteId && x.DilId == dilId).OrderByDescending(x => x.YayimTarihi)
                .ToListAsync(cancellationToken);
        }
    }
}
