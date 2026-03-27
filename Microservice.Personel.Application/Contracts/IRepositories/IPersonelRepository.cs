namespace Microservice.Personel.Application.Contracts.IRepositories
{
    public interface IPersonelRepository: IGenericRepository<Domain.Entities.Personel>
    {
        Task<Domain.Entities.Personel> GetPersonelByUsername(string username);
    }
}
