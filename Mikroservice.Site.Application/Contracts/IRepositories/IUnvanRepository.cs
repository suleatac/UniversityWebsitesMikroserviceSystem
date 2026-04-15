using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IUnvanRepository : IGenericRepository<Unvan>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
