using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Persistence.Repositories
{
    public class IcerikRepository : GenericRepository<Icerik>, IIcerikRepository
    {
        private readonly AppDbContext _appDbContext;

        public IcerikRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<Icerik>().AnyAsync(cancellationToken);
        }
    }
}
