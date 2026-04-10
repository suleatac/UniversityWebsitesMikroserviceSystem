using Microservice.Yonetici.Application.Contracts.IRepositories;
using Microservice.Yonetici.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Yonetici.Persistence.Repositories
{
    public class YoneticiDuyuruRepository : GenericRepository<YoneticiDuyuru>, IYoneticiDuyuruRepository
    {
        private readonly AppDbContext _appDbContext;

        public YoneticiDuyuruRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.YoneticiDuyurular.AnyAsync(cancellationToken);
        }


    }
}