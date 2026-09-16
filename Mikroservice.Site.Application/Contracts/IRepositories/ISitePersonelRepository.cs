using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface ISitePersonelRepository : IGenericRepository<SitePersonel>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task<bool> AnyByUnvanIdAsync(int unvanId, CancellationToken cancellationToken = default);
        Task<List<SitePersonel>> GetAllWithPersonelTipAndUnvanAsync(int siteId, CancellationToken cancellationToken = default);
        Task<bool> IsSeoUrlAvailableAsync(int siteId, int PageTypeId, string SeoUrl, int? ExcludeIcerikId, CancellationToken cancellationToken = default);
        Task<SitePersonel?> GetBySeoUrlAsync(int siteId, string seoUrl, CancellationToken cancellationToken = default);
    }
}
