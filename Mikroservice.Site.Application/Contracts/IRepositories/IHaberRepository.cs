using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IHaberRepository : IGenericRepository<Haber>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task<Haber?> GetBySeoUrlAsync(int siteId, int dilId, string seoUrl, CancellationToken cancellationToken = default);
        Task<Haber?> GetByIdWithPageTypeAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Haber>> GetBySiteAndLanguageAsync(int siteId, int dilId, CancellationToken cancellationToken);
    }
}

