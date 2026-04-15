using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Persistence.Repositories
{
    public class BilgiRepository : GenericRepository<Bilgi>, IBilgiRepository
    {
        private readonly AppDbContext _appDbContext;

        public BilgiRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<Bilgi>().AnyAsync(cancellationToken);
        }
    }
}
