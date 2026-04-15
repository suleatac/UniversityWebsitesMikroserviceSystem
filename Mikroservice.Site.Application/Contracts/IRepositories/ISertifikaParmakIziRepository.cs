using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface ISertifikaParmakIziRepository : IGenericRepository<SertifikaParmakIzi>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
