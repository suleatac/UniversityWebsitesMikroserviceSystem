using Microservice.Site.Persistence;
using Microservice.Site.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Repositories
{
    public class PageSectionRepository : GenericRepository<PageSection>, IPageSectionRepository
    {
        private readonly AppDbContext _appDbContext;

        public PageSectionRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<PageSection>> GetPageSectionsAsync(
            int siteId,
            int dilId,
            bool publishedOnly,
            CancellationToken cancellationToken = default)
        {
            var query = _appDbContext.Set<PageSection>()
                .Where(s => s.SiteId == siteId && s.DilId == dilId);

            if (publishedOnly)
            {
                query = query.Where(s => s.Yayinda);
            }

            return await query
                .OrderBy(s => s.Sira)
                .Include(s => s.Blocks)
                    .ThenInclude(b => b.Medias)
                .Include(s => s.Blocks)
                    .ThenInclude(b => b.Children)
                        .ThenInclude(c => c.Medias)
                .ToListAsync(cancellationToken);
        }

        public async Task<PageSection?> GetPageSectionByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<PageSection>()
                .Where(s => s.Id == id)
                .Include(s => s.Blocks)
                    .ThenInclude(b => b.Medias)
                .Include(s => s.Blocks)
                    .ThenInclude(b => b.Children)
                        .ThenInclude(c => c.Medias)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
