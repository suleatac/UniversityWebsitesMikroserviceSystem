namespace Microservice.Ogrenci.Application.Contracts.IRepositories
{
    public interface IOgrenciRepository: IGenericRepository<Domain.Entities.Ogrenci>
    {
        Task<List<Domain.Entities.Ogrenci>> GetOgrencis();
        Task<Microservice.Ogrenci.Domain.Entities.Ogrenci> GetOgrenciByOgrenciProgramId(int ogrenciprogramid);
    }
}
