using Microservice.Ogrenci.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Mikroservice.Ogrenci.Persistence.Repositories
{
    public class OgrenciRepository(AppDbContext appDbContext) : GenericRepository<Microservice.Ogrenci.Domain.Entities.Ogrenci>(appDbContext), IOgrenciRepository
    {
     
        public async Task<Microservice.Ogrenci.Domain.Entities.Ogrenci?> GetOgrenciByOgrenciProgramId(int? ogrenciProgramId, CancellationToken cancellationToken = default)
        {
            return await appDbContext.Ogrencis
                .FirstOrDefaultAsync(x => x.OgrenciProgramId == ogrenciProgramId, cancellationToken);
        }
        
        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await appDbContext.Ogrencis.AnyAsync(cancellationToken);
        }

    
    }
}
