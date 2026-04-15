using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IBilgiRepository : IGenericRepository<Bilgi>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
