using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface ISiteRepository : IGenericRepository<Site>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
