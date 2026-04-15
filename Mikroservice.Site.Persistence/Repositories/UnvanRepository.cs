using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Persistence.Repositories
{
    public class UnvanRepository : GenericRepository<Unvan>, IUnvanRepository
    {
        private readonly AppDbContext _appDbContext;

        public UnvanRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<Unvan>().AnyAsync(cancellationToken);
        }
    }
}
