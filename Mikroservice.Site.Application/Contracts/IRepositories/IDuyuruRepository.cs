using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IDuyuruRepository : IGenericRepository<Duyuru>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
