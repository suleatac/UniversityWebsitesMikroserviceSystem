using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface ISikcaSorulanSoruRepository : IGenericRepository<SikcaSorulanSoru>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
