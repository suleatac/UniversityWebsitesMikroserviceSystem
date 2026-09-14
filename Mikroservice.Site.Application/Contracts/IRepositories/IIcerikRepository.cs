using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IIcerikRepository : IGenericRepository<Icerik>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task<bool> IsSeoUrlAvailableAsync(int siteId, int PageTypeId, string SeoUrl, int? ExcludeIcerikId, CancellationToken cancellationToken = default);
    }
}
