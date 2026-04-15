using Microservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IYonetimDuyuruRepository : IGenericRepository<YonetimDuyuru>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
