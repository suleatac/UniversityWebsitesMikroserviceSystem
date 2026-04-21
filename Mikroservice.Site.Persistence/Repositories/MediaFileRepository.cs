using Microservice.Site.Persistence;
using Microservice.Site.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Repositories
{
    public class MediaFileRepository : GenericRepository<MediaFile>, IMediaFileRepository
    {
        private readonly AppDbContext _appDbContext;

        public MediaFileRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<MediaFile>().AnyAsync(cancellationToken);
        }
    }
}
