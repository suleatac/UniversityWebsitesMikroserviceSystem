using Microservice.Site.Persistence;
using Microservice.Site.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Repositories
{
    public class PageBlockRepository : GenericRepository<PageBlock>, IPageBlockRepository
    {
        private readonly AppDbContext _appDbContext;

        public PageBlockRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<PageBlock>> GetBlocksBySectionAsync(int pageSectionId, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<PageBlock>()
                .Where(b => b.PageSectionId == pageSectionId)
                .OrderBy(b => b.RowNumber)
                .Include(b => b.Medias)
                .Include(b => b.Children)
                    .ThenInclude(c => c.Medias)
                .ToListAsync(cancellationToken);
        }

        public async Task<PageBlock?> GetTrackedWithMediasAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<PageBlock>()
                .Where(b => b.Id == id)
                .Include(b => b.Medias)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
