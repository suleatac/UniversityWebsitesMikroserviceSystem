using Microservice.Site.Domain.Entities;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Site.Persistence.Repositories
{
    public class YonetimDuyuruOkunduRepository : GenericRepository<YonetimDuyuruOkundu>, IYonetimDuyuruOkunduRepository
    {
        private readonly AppDbContext _appDbContext;

        public YonetimDuyuruOkunduRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> HasUserReadAsync(int yonetimDuyuruId, string keycloakUserId, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<YonetimDuyuruOkundu>()
                .AnyAsync(x => x.YonetimDuyuruId == yonetimDuyuruId && x.KeycloakUserId == keycloakUserId, cancellationToken);
        }

        public async Task<int> GetUnreadCountForUserAsync(string keycloakUserId, CancellationToken cancellationToken = default)
        {
            var totalDuyuruCount = await _appDbContext.Set<YonetimDuyuru>()
                .Where(x => !x.IsDeleted)
                .CountAsync(cancellationToken);

            var readCount = await _appDbContext.Set<YonetimDuyuruOkundu>()
                .Where(x => x.KeycloakUserId == keycloakUserId)
                .Select(x => x.YonetimDuyuruId)
                .Distinct()
                .CountAsync(cancellationToken);

            return totalDuyuruCount - readCount;
        }

        public async Task<YonetimDuyuruOkundu?> GetByDuyuruIdAndUserIdAsync(int yonetimDuyuruId, string keycloakUserId, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<YonetimDuyuruOkundu>()
                .FirstOrDefaultAsync(x => x.YonetimDuyuruId == yonetimDuyuruId && x.KeycloakUserId == keycloakUserId, cancellationToken);
        }
    }
}
