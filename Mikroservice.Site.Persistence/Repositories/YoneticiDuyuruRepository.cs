using Microservice.Site.Application.Contracts.IRepositories;
using Microservice.Site.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Site.Persistence.Repositories
{
    public class YoneticiDuyuruRepository : GenericRepository<YonetimDuyuru>, IYoneticiDuyuruRepository
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