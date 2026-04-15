using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IHaberRepository : IGenericRepository<Haber>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
