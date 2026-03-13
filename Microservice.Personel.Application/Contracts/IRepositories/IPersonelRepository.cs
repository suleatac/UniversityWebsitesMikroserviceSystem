namespace Microservice.Personel.Application.Contracts.IRepositories
{
    public interface IPersonelRepository: IGenericRepository<Microservice.Personel.Domain.Entities.Personel>
    {
        Task<List<Domain.Entities.Personel>> GetPersonels();
        Task<Domain.Entities.Personel> GetPersonelByUsername(string username);
    }
}
