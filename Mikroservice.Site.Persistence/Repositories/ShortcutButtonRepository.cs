using Microservice.Site.Persistence;
using Microservice.Site.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Repositories
{
    public class ShortcutButtonRepository : GenericRepository<ShortcutButton>, IShortcutButtonRepository
    {
        private readonly AppDbContext _appDbContext;

        public ShortcutButtonRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<ShortcutButton>().AnyAsync(cancellationToken);
        }
        public Task<List<ShortcutButton>> GetShortcutButtonsBySiteAndDilAsync(int siteId, int dilId, CancellationToken cancellationToken = default)
        {
            var data = _appDbContext.Set<ShortcutButton>()
                .Where(b => b.SiteId == siteId && b.DilId == dilId && !b.IsDeleted)
                .OrderBy(b => b.Sira)
                .ToListAsync(cancellationToken);

            return data;
        }
    }
}
