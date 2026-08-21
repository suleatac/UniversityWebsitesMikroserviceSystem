using Microservice.Site.Persistence;
using Microservice.Site.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Repositories
{
    public class PageRouteRepository : GenericRepository<PageRoute>, IPageRouteRepository
    {
        private readonly AppDbContext _appDbContext;

        public PageRouteRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<PageRoute?> GetBySlugAsync(int siteId,
        string slug,CancellationToken cancellationToken = default)
        {
            return await _appDbContext.PageRoutes
             .AsNoTracking()
             .Include(x => x.PageType)
             .Include(x => x.SeoMetadata)
             .FirstOrDefaultAsync(x =>
                 x.SiteId == siteId &&
                 x.IsActive &&
                 x.Slug == slug, cancellationToken);
        }
    }
}
