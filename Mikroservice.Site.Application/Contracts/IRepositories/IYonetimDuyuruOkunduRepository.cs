using Microservice.Site.Domain.Entities;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IYonetimDuyuruOkunduRepository : IGenericRepository<YonetimDuyuruOkundu>
    {
        Task<bool> HasUserReadAsync(int yonetimDuyuruId, string keycloakUserId, CancellationToken cancellationToken = default);
        Task<int> GetUnreadCountForUserAsync(string keycloakUserId, CancellationToken cancellationToken = default);
        Task<YonetimDuyuruOkundu?> GetByDuyuruIdAndUserIdAsync(int yonetimDuyuruId, string keycloakUserId, CancellationToken cancellationToken = default);
    }
}
