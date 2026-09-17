using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Persistence.Repositories
{
    public class IcerikResimRepository : GenericRepository<IcerikResim>, IIcerikResimRepository
    {
        private readonly AppDbContext _appDbContext;

        public IcerikResimRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<IcerikResim>> GetByIcerikIdAsync(int icerikId, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<IcerikResim>()
                .AsNoTracking()
                .Where(x => x.IcerikId == icerikId)
                .OrderBy(x => x.Sira)
                .ThenBy(x => x.Id)
                .ToListAsync(cancellationToken);
        }

        // Sync islemi icin tracked (degisiklik kaydedilebilir) resimler
        public async Task<List<IcerikResim>> GetTrackedByIcerikIdsAsync(IEnumerable<int> icerikIds, CancellationToken cancellationToken = default)
        {
            var ids = icerikIds.Distinct().ToList();

            return await _appDbContext.Set<IcerikResim>()
                .Where(x => ids.Contains(x.IcerikId))
                .ToListAsync(cancellationToken);
        }
    }
}
