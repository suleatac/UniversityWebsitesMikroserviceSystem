namespace Microservice.Ogrenci.Application.Contracts.IRepositories
{
    public interface IOgrenciRepository: IGenericRepository<Domain.Entities.Ogrenci>
    {
   
        Task<Microservice.Ogrenci.Domain.Entities.Ogrenci> GetOgrenciByOgrenciProgramId(int ogrenciprogramid);
    }
}
