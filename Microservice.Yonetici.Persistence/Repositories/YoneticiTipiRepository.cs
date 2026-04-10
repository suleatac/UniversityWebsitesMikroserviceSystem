using Microservice.Yonetici.Application.Contracts.IRepositories;
using Microservice.Yonetici.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Yonetici.Persistence.Repositories
{
    public class YoneticiTipiRepository : GenericRepository<YoneticiTipi>, IYoneticiTipiRepository
    {
        private readonly AppDbContext _appDbContext;

        public YoneticiTipiRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<YoneticiTipi?> GetYoneticiTipiByValue(int value, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.YoneticiTipleri.FirstOrDefaultAsync(x => x.Value == value, cancellationToken);
        }
        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.YoneticiTipleri.AnyAsync(cancellationToken);
        }



    }
}
