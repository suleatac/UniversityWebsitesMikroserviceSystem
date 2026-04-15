using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Persistence.Repositories
{
    public class DuyuruRepository : GenericRepository<Duyuru>, IDuyuruRepository
    {
        private readonly AppDbContext _appDbContext;

        public DuyuruRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<Duyuru>().AnyAsync(cancellationToken);
        }
    }
}
