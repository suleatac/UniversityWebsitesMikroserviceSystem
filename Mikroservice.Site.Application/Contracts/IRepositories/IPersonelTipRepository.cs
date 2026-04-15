using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IPersonelTipRepository : IGenericRepository<PersonelTip>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
