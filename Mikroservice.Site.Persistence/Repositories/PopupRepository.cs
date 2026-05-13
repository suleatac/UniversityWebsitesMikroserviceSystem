using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;
using Microservice.Site.Persistence.Repositories;

namespace Microservice.Site.Persistence.Repositories
{
    public class PopupRepository : GenericRepository<Popup>, IPopupRepository
    {
        private readonly AppDbContext _appDbContext;

        public PopupRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<Popup>().AnyAsync(cancellationToken);
        }

        public async Task<Popup?> GetBySiteIdAsync(int siteId)
        {
            return await _appDbContext.Set<Popup>()
                .FirstOrDefaultAsync(p => p.SiteId == siteId);
        }
    }
}
