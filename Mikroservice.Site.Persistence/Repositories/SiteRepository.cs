using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Site.Persistence.Repositories
{
    public class SiteRepository : GenericRepository<Mikroservice.Site.Domain.Entities.Site>, ISiteRepository
    {
        private readonly AppDbContext _appDbContext;

        public SiteRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<Mikroservice.Site.Domain.Entities.Site>().AnyAsync(cancellationToken);
        }
    }
}
