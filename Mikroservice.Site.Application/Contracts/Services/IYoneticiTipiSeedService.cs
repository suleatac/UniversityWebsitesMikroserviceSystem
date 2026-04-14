namespace Microservice.Site.Application.Contracts.Services
{
    public interface IYoneticiTipiSeedService
    {
        Task<bool> IsDatabaseSeededAsync(CancellationToken cancellationToken = default);
        Task SeedInitialDataAsync(CancellationToken cancellationToken = default);
    }
}
