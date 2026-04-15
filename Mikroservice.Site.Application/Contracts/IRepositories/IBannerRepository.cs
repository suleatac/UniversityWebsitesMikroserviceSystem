using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IBannerRepository : IGenericRepository<Banner>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
