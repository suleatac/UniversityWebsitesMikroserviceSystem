using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface ISitePersonelRepository : IGenericRepository<SitePersonel>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task<bool> AnyByUnvanIdAsync(int unvanId, CancellationToken cancellationToken = default);
        Task<List<SitePersonel>> GetAllWithPersonelTipPageTypeAndUnvanAsync(int siteId, CancellationToken cancellationToken = default);

        // Unique index (SiteId, SeoUrl) ile birebir eslesir: sayfa tipi filtresi YOKTUR.
        // true => slug ALINMIS (carpisma var). ExcludeSitePersonelId guncellemede kendisini muaf tutar.
        Task<bool> IsSeoUrlTakenAsync(int siteId, string SeoUrl, int? ExcludeSitePersonelId, CancellationToken cancellationToken = default);
        Task<SitePersonel?> GetBySeoUrlAsync(int siteId, string seoUrl, CancellationToken cancellationToken = default);
    }
}
