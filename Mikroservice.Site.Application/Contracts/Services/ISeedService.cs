namespace Mikroservice.Site.Application.Contracts.Services
{
    public interface ISeedService
    {
        int Sira { get; }
        Task<bool> IsDatabaseSeededAsync(CancellationToken cancellationToken = default);
        Task SeedInitialDataAsync(CancellationToken cancellationToken = default);
    }
}
