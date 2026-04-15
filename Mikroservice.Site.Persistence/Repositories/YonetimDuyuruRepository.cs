using Microservice.Site.Application.Contracts.IRepositories;
using Microservice.Site.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Site.Persistence.Repositories
{
    public class YonetimDuyuruRepository : GenericRepository<YonetimDuyuru>, IYonetimDuyuruRepository
    {
        private readonly AppDbContext _appDbContext;

        public YonetimDuyuruRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<YonetimDuyuru>().AnyAsync(cancellationToken);
        }
    }
}
