using Mikroservice.Site.Domain.Entities;
using Mikroservice.Site.Domain.Enums;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IMenuRepository : IGenericRepository<Menu>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);

        // Site + dil (+ bolge) bazinda, PageType bilgisi eager loading ile menu listesi
        Task<List<Menu>> GetMenusWithPageTypeAsync(int siteId, int dilId, MenuLocation? location, CancellationToken cancellationToken = default);
        Task<Menu?> GetBySeoUrlAsync(int siteId, int dilId, string seoUrl, CancellationToken cancellationToken = default);

        // Id bazinda, PageType bilgisi eager loading ile tek menu
        Task<Menu?> GetByIdWithPageTypeAsync(int id, CancellationToken cancellationToken = default);
    }
}
