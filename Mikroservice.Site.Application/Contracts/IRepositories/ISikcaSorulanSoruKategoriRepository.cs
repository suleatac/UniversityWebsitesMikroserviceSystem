using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface ISikcaSorulanSoruKategoriRepository : IGenericRepository<SikcaSorulanSoruKategori>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
