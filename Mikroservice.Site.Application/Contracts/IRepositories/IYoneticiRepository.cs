using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IYoneticiRepository : IGenericRepository<Yonetici>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
