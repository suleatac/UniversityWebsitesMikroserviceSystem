using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IIcerikRepository : IGenericRepository<Icerik>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
