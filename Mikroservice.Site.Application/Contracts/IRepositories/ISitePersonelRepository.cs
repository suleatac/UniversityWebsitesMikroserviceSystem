using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface ISitePersonelRepository : IGenericRepository<SitePersonel>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task<bool> AnyByUnvanIdAsync(int unvanId, CancellationToken cancellationToken = default);
        Task<List<SitePersonel>> GetAllWithPersonelTipAndUnvanAsync(int siteId, CancellationToken cancellationToken = default);
    }
}
