using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Persistence.Repositories
{
    public class SikcaSorulanSoruRepository : GenericRepository<SikcaSorulanSoru>, ISikcaSorulanSoruRepository
    {
        private readonly AppDbContext _appDbContext;

        public SikcaSorulanSoruRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<SikcaSorulanSoru>().AnyAsync(cancellationToken);
        }
    }
}
