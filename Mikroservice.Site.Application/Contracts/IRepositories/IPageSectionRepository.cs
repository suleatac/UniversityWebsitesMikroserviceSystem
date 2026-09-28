using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Contracts.IRepositories
{
    public interface IPageSectionRepository : IGenericRepository<PageSection>
    {
        /// <summary>
        /// Site+dil bazinda bolumleri (block+media+child agaciyla) dondurur.
        /// publishedOnly true ise yalnizca Yayinda olanlar (web kullanimi).
        /// </summary>
        Task<List<PageSection>> GetPageSectionsAsync(int siteId, int dilId, bool publishedOnly, CancellationToken cancellationToken = default);

        /// <summary>Bolumu blocklari ve medyalariyla birlikte id ile dondurur (admin).</summary>
        Task<PageSection?> GetPageSectionByIdAsync(int id, CancellationToken cancellationToken = default);
    }

}
