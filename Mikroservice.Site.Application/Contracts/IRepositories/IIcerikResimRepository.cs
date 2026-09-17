using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IIcerikResimRepository : IGenericRepository<IcerikResim>
    {
        // Icerik bazinda, slider'a gelecek aktif resimler
        Task<List<IcerikResim>> GetByIcerikIdAsync(int icerikId, CancellationToken cancellationToken = default);

        // Sync (esitleme) islemi icin tracked resimler
        Task<List<IcerikResim>> GetTrackedByIcerikIdsAsync(IEnumerable<int> icerikIds, CancellationToken cancellationToken = default);
    }
}
