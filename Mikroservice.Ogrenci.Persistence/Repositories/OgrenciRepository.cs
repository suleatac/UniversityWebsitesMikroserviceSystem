using Microservice.Ogrenci.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Mikroservice.Ogrenci.Persistence.Repositories
{
    public class OgrenciRepository(AppDbContext appDbContext) : GenericRepository<Microservice.Ogrenci.Domain.Entities.Ogrenci>(appDbContext), IOgrenciRepository
    {
        public async Task<List<Microservice.Ogrenci.Domain.Entities.Ogrenci>> GetOgrencis()
        {
            // await eklendi
            List<Microservice.Ogrenci.Domain.Entities.Ogrenci> personels = await appDbContext.Ogrencis.ToListAsync();

            return personels;
        }
        public async Task<Microservice.Ogrenci.Domain.Entities.Ogrenci> GetOgrenciByOgrenciProgramId(int ogrenciprogramid)
        {
            // await eklendi
           var personel = await appDbContext.Ogrencis.Where(s=>s.ogrenciprogramid==ogrenciprogramid).FirstOrDefaultAsync();

           return personel;
        }

    }
}
