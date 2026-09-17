using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IIcerikDosyaRepository : IGenericRepository<IcerikDosya>
    {
        // Site+icerik bazinda, listeye/sidebara gelecek aktif dosyalar
        Task<List<IcerikDosya>> GetByIcerikIdAsync(int icerikId, CancellationToken cancellationToken = default);

        // Sync (esitleme) islemi icin tracked dosyalar
        Task<List<IcerikDosya>> GetTrackedByIcerikIdsAsync(IEnumerable<int> icerikIds, CancellationToken cancellationToken = default);
    }
}
