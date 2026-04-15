using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Persistence.Repositories
{
    public class SikcaSorulanSoruKategoriRepository : GenericRepository<SikcaSorulanSoruKategori>, ISikcaSorulanSoruKategoriRepository
    {
        private readonly AppDbContext _appDbContext;

        public SikcaSorulanSoruKategoriRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<SikcaSorulanSoruKategori>().AnyAsync(cancellationToken);
        }
    }
}
