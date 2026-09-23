using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IIcerikRepository : IGenericRepository<Icerik>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);

        // Unique index (SiteId, SeoUrl) ile birebir eslesir: sayfa tipi/ dil filtresi YOKTUR.
        // true => slug ALINMIS (carpisma var). ExcludeIcerikId guncellemede kendisini muaf tutar.
        Task<bool> IsSeoUrlTakenAsync(int siteId, string SeoUrl, int? ExcludeIcerikId, CancellationToken cancellationToken = default);
    }
}
