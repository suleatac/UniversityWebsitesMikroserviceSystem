using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Persistence.Repositories
{
    public class IcerikRepository : GenericRepository<Icerik>, IIcerikRepository
    {
        private readonly AppDbContext _appDbContext;

        public IcerikRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<Icerik>().AnyAsync(cancellationToken);
        }
        public Task<bool> IsSeoUrlTakenAsync(int siteId, string SeoUrl, int? ExcludeIcerikId, CancellationToken cancellationToken = default)
        {
            // Unique index "IX_Icerik_SiteId_SeoUrl" (SiteId, SeoUrl) uzerindedir ve
            // sayfa tipi/dil ayrimi yapmaz. PageTypeId filtresi buradan kaldirildi;
            // aksi halde baska bir icerik tipinin aldigi slug "musait" gorunup
            // SaveChanges'te 23505 (unique violation) firlatiyordu.
            return _appDbContext.Set<Icerik>()
                .AnyAsync(x => x.SiteId == siteId
                                && x.SeoUrl == SeoUrl
                                && (!ExcludeIcerikId.HasValue || x.Id != ExcludeIcerikId.Value),
                    cancellationToken);
        }
    }
}
