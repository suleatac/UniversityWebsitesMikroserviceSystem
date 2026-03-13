using Microservice.Personel.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Mikroservice.Personel.Persistence.Repositories
{
    public class PersonelRepository(AppDbContext appDbContext) : GenericRepository<Microservice.Personel.Domain.Entities.Personel>(appDbContext), IPersonelRepository
    {
        public async Task<List<Microservice.Personel.Domain.Entities.Personel>> GetPersonels()
        {
            // await eklendi
            List<Microservice.Personel.Domain.Entities.Personel> personels = await appDbContext.Personels.ToListAsync();

            return personels;
        }
        public async Task<Microservice.Personel.Domain.Entities.Personel> GetPersonelByUsername(string username)
        {
            // await eklendi
           var personel = await appDbContext.Personels.Where(s=>s.username==username).FirstOrDefaultAsync();

           return personel;
        }

    }
}
