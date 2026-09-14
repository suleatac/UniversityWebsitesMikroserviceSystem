using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Persistence.Repositories
{
    public class BannerRepository : GenericRepository<Banner>, IBannerRepository
    {
        private readonly AppDbContext _appDbContext;

        public BannerRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<Banner>().AnyAsync(cancellationToken);
        }
        public async Task<Banner?> GetByIdWithPageTypeAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<Banner>()
                .AsNoTracking()
                .Include(x => x.PageType) // eager loading
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public async Task<List<Banner>> GetBySiteAndLanguageAsync(int siteId, int dilId, CancellationToken cancellationToken)
        {
            return await _appDbContext
                .Set<Banner>()
                .AsNoTracking()
                .Include(x => x.PageType) // eager loading
                .Where(x => x.SiteId == siteId && x.DilId == dilId).OrderBy(x => x.Sira)
                .ToListAsync(cancellationToken);
        }
        public Task<Banner?> GetBySeoUrlAsync(int siteId, int dilId, string seoUrl, CancellationToken cancellationToken = default)
        {
            return _appDbContext.Set<Banner>()
                .AsNoTracking()
                .Include(x => x.PageType) // eager loading
                .FirstOrDefaultAsync(x => x.SiteId == siteId && x.DilId == dilId && x.SeoUrl == seoUrl, cancellationToken);
        }
    }
}
