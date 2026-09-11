using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;
using Mikroservice.Site.Domain.Enums;

namespace Microservice.Site.Persistence.Repositories
{
    public class MenuRepository : GenericRepository<Menu>, IMenuRepository
    {
        private readonly AppDbContext _appDbContext;

        public MenuRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<Menu>().AnyAsync(cancellationToken);
        }

        public async Task<List<Menu>> GetMenusWithPageTypeAsync(int siteId, int dilId, MenuLocation? location, CancellationToken cancellationToken = default)
        {
            var query = _appDbContext.Set<Menu>()
                .AsNoTracking()
                .Include(x => x.PageType) // eager loading
                .Where(x =>
                    x.SiteId == siteId &&
                    x.DilId == dilId &&
                    x.IsVisible &&
                    !x.IsDeleted);

            if (location.HasValue)
            {
                query = query.Where(x => x.Location == location.Value);
            }

            return await query
                .OrderBy(x => x.Sira)
                .ToListAsync(cancellationToken);
        }

        public Task<Menu?> GetBySeoUrlAsync(int siteId, int dilId, string seoUrl, CancellationToken cancellationToken = default)
        {
            return _appDbContext.Set<Menu>()
                .FirstOrDefaultAsync(x => x.SiteId == siteId && x.DilId == dilId && x.SeoUrl == seoUrl, cancellationToken);
        }

        public async Task<Menu?> GetByIdWithPageTypeAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<Menu>()
                .AsNoTracking()
                .Include(x => x.PageType) // eager loading
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
