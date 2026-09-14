using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IVideoRepository : IGenericRepository<Video>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task<Video?> GetByIdWithPageTypeAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Video>> GetBySiteAndLanguageAsync(int siteId, int dilId, CancellationToken cancellationToken);
        Task<Video?> GetBySeoUrlAsync(int siteId, int dilId, string seoUrl, CancellationToken cancellationToken = default);
    }
}

