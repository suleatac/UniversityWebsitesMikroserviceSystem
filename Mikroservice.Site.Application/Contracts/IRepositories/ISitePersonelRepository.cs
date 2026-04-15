using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface ISitePersonelRepository : IGenericRepository<SitePersonel>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
