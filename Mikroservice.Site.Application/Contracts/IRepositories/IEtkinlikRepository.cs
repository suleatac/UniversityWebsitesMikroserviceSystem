using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IEtkinlikRepository : IGenericRepository<Etkinlik>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
