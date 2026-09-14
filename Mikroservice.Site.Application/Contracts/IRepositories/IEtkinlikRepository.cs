using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IEtkinlikRepository : IGenericRepository<Etkinlik>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task<Etkinlik?> GetByIdWithPageTypeAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Etkinlik>> GetBySiteAndLanguageAsync(int siteId, int dilId, CancellationToken cancellationToken);
        Task<Etkinlik?> GetBySeoUrlAsync(int siteId, int dilId, string seoUrl, CancellationToken cancellationToken = default);
    }
}

