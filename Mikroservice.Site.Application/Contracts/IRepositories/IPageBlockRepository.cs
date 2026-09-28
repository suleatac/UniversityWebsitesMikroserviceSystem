using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Contracts.IRepositories
{
    public interface IPageBlockRepository : IGenericRepository<PageBlock>
    {
        /// <summary>Bolumun tum (soft-delete hariç) containerlarini media/child Include ile dondurur.</summary>
        Task<List<PageBlock>> GetBlocksBySectionAsync(int pageSectionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Block'u medyalariyla birlikte TRACKED getirir (Update senkronizasyonu icin).
        /// </summary>
        Task<PageBlock?> GetTrackedWithMediasAsync(int id, CancellationToken cancellationToken = default);
    }

}
