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
        public Task<bool> IsSeoUrlAvailableAsync(int siteId, int PageTypeId, string SeoUrl, int? ExcludeIcerikId, CancellationToken cancellationToken = default)
        {
            return _appDbContext.Set<Icerik>()
                .AnyAsync(x => x.SiteId == siteId
                                && x.SeoUrl == SeoUrl
                                && x.PageTypeId == PageTypeId
                                && (!ExcludeIcerikId.HasValue || x.Id != ExcludeIcerikId.Value),
                    cancellationToken);
        }
    }
}
