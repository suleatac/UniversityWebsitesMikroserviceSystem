using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Persistence.Repositories
{
    public class SitePersonelRepository : GenericRepository<SitePersonel>, ISitePersonelRepository
    {
        private readonly AppDbContext _appDbContext;

        public SitePersonelRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<SitePersonel>().AnyAsync(cancellationToken);
        }
        public async Task<bool> AnyByUnvanIdAsync(int unvanId,CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<SitePersonel>().AnyAsync(x=>x.UnvanId==unvanId,cancellationToken);
        }
        public async Task<List<SitePersonel>> GetAllWithPersonelTipPageTypeAndUnvanAsync(int siteId,CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<SitePersonel>()
                .Include(x => x.Unvan).Include(x => x.PersonelTip).Include(x => x.PageType)
                .Where(x => x.SiteId == siteId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        public Task<bool> IsSeoUrlTakenAsync(int siteId, string SeoUrl, int? ExcludeSitePersonelId, CancellationToken cancellationToken = default)
        {
            // Unique index "IX_SitePersonelleri_SiteId_SeoUrl" (SiteId, SeoUrl) uzerindedir.
            // Eskiden buraya Icerik tablosunun PK'si (ExcludeIcerikId) veriliyordu; farkli
            // tablonun Id'siyle eslesen personel kaydi yanlislikla muaf sayiliyordu.
            // Artik yalnizca SitePersonel Id exclude edilir.
            return _appDbContext.Set<SitePersonel>()
                .AnyAsync(x => x.SiteId == siteId
                                && x.SeoUrl == SeoUrl
                                && (!ExcludeSitePersonelId.HasValue || x.Id != ExcludeSitePersonelId.Value),
                    cancellationToken);
        }

        public Task<SitePersonel?> GetBySeoUrlAsync(int siteId, string seoUrl, CancellationToken cancellationToken = default)
        {
            return _appDbContext.Set<SitePersonel>()
                .AsNoTracking()
                .Include(x => x.PageType) // eager loading
                .FirstOrDefaultAsync(x => x.SiteId == siteId && x.SeoUrl == seoUrl, cancellationToken);
        }

    }
}
