using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Persistence.Repositories
{
    public class IcerikDosyaRepository : GenericRepository<IcerikDosya>, IIcerikDosyaRepository
    {
        private readonly AppDbContext _appDbContext;

        public IcerikDosyaRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<IcerikDosya>> GetByIcerikIdAsync(int icerikId, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<IcerikDosya>()
                .AsNoTracking()
                .Where(x => x.IcerikId == icerikId)
                .OrderBy(x => x.Sira)
                .ThenBy(x => x.Id)
                .ToListAsync(cancellationToken);
        }

        // Sync islemi icin tracked (degisiklik kaydedilebilir) dosyalar
        public async Task<List<IcerikDosya>> GetTrackedByIcerikIdsAsync(IEnumerable<int> icerikIds, CancellationToken cancellationToken = default)
        {
            var ids = icerikIds.Distinct().ToList();

            return await _appDbContext.Set<IcerikDosya>()
                .Where(x => ids.Contains(x.IcerikId))
                .ToListAsync(cancellationToken);
        }
    }
}
