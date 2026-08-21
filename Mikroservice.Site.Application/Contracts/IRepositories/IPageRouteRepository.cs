using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Contracts.IRepositories
{
    public interface IPageRouteRepository : IGenericRepository<PageRoute>
    {
        Task<PageRoute?> GetBySlugAsync(int siteId,
        string slug, CancellationToken cancellationToken = default);
    }
}
