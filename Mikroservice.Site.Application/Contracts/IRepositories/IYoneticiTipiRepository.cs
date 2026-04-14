using Microservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IYoneticiTipiRepository : IGenericRepository<YoneticiTipi>
    {
        Task<YoneticiTipi?> GetYoneticiTipiByValue(int value, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
