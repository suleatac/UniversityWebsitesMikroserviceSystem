using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IPersonelTelefonRepository : IGenericRepository<PersonelTelefon>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
