using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Persistence.Repositories
{
    public class PersonelTipRepository : GenericRepository<PersonelTip>, IPersonelTipRepository
    {
        private readonly AppDbContext _appDbContext;

        public PersonelTipRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Set<PersonelTip>().AnyAsync(cancellationToken);
        }
    }
}
