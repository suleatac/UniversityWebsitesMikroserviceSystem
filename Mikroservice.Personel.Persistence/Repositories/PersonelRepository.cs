using Microservice.Personel.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Mikroservice.Personel.Persistence.Repositories
{
    public class PersonelRepository(AppDbContext appDbContext) : GenericRepository<Microservice.Personel.Domain.Entities.Personel>(appDbContext), IPersonelRepository
    {
  
        public async Task<Microservice.Personel.Domain.Entities.Personel?> GetPersonelByUsername(string username,CancellationToken cancellationToken = default)
        {
            return await appDbContext.Personels
                .FirstOrDefaultAsync(x => x.username == username, cancellationToken);
        }
        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await appDbContext.Personels.AnyAsync(cancellationToken);
        }

    }
}
