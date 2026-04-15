using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IDilRepository : IGenericRepository<Dil>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
