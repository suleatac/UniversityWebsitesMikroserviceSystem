using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IBirimRepository : IGenericRepository<Birim>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
