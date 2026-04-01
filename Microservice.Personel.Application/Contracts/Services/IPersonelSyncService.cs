using Microservice.Personel.Application.Contracts.DTOs;

namespace Microservice.Personel.Application.Contracts.Services
{
    public interface IPersonelSyncService
    {
        Task<PersonelSyncResponse> SyncPersonelsAsync(DateTime? lastUpdateDate = null, CancellationToken cancellationToken = default);
    }
}
