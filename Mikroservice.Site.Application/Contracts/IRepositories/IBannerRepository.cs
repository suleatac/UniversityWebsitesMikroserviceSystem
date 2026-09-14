using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IBannerRepository : IGenericRepository<Banner>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task<Banner?> GetByIdWithPageTypeAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Banner>> GetBySiteAndLanguageAsync(int siteId, int dilId, CancellationToken cancellationToken);
        Task<Banner?> GetBySeoUrlAsync(int siteId, int dilId, string seoUrl, CancellationToken cancellationToken = default);
    }
}

