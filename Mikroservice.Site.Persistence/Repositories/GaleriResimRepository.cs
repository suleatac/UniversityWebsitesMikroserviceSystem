using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Persistence.Repositories
{
    public class GaleriResimRepository : GenericRepository<GaleriResim>, IGaleriResimRepository
    {
        private readonly AppDbContext _appDbContext;

        public GaleriResimRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<GaleriResim>().AnyAsync(cancellationToken);
        }
        public async Task<GaleriResim?> GetByIdWithPageTypeAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<GaleriResim>()
                .AsNoTracking()
                .Include(x => x.PageType) // eager loading
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public async Task<List<GaleriResim>> GetBySiteAndLanguageAsync(int siteId, int dilId, CancellationToken cancellationToken)
        {
            return await _appDbContext
                .Set<GaleriResim>()
                .AsNoTracking()
                .Include(x => x.PageType) // eager loading
                .Where(x => x.SiteId == siteId && x.DilId == dilId)
                .OrderBy(x => x.Sira)
                .ThenByDescending(x => x.YayimTarihi)
                .ToListAsync(cancellationToken);
        }
        public Task<GaleriResim?> GetBySeoUrlAsync(int siteId, int dilId, string seoUrl, CancellationToken cancellationToken = default)
        {
            return _appDbContext.Set<GaleriResim>()
                .AsNoTracking()
                .Include(x => x.PageType) // eager loading
                .FirstOrDefaultAsync(x => x.SiteId == siteId && x.DilId == dilId && x.SeoUrl == seoUrl, cancellationToken);
        }
    }
}
