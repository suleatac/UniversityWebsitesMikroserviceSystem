using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IYoneticiSiteRepository : IGenericRepository<YoneticiSite>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
