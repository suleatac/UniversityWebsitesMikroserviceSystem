using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IGaleriResimRepository : IGenericRepository<GaleriResim>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task<GaleriResim?> GetByIdWithPageTypeAsync(int id, CancellationToken cancellationToken = default);
        Task<List<GaleriResim>> GetBySiteAndLanguageAsync(int siteId, int dilId, CancellationToken cancellationToken);
        Task<GaleriResim?> GetBySeoUrlAsync(int siteId, int dilId, string seoUrl, CancellationToken cancellationToken = default);
    }
}
