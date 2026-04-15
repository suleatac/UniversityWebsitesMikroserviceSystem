using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IBandLogoRepository : IGenericRepository<BandLogo>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}