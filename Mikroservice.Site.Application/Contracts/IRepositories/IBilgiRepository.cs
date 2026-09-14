using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IBilgiRepository : IGenericRepository<Bilgi>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task<Bilgi?> GetByIdWithPageTypeAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Bilgi>> GetBySiteAndLanguageAsync(int siteId, int dilId, CancellationToken cancellationToken);
        Task<Bilgi?> GetBySeoUrlAsync(int siteId, int dilId, string seoUrl, CancellationToken cancellationToken = default);
    }
}
