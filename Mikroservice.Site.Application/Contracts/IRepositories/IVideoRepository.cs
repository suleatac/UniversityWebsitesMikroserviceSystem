using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IVideoRepository : IGenericRepository<Video>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
