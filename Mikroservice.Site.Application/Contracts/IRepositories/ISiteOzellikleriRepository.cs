using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface ISiteOzellikleriRepository : IGenericRepository<SiteOzellikleri>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
