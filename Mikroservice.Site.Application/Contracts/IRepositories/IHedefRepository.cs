using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IHedefRepository : IGenericRepository<Hedef>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
