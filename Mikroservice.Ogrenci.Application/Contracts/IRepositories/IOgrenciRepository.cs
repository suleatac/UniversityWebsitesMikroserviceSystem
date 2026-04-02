namespace Microservice.Ogrenci.Application.Contracts.IRepositories
{
    public interface IOgrenciRepository: IGenericRepository<Domain.Entities.Ogrenci>
    {

        Task<Domain.Entities.Ogrenci?> GetOgrenciByOgrenciProgramId(int? ogrenciProgramId, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
