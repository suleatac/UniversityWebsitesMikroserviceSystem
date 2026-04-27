namespace Microservice.Admin.Services
{
    public interface IRedisCacheService
    {
        Task SetAsync<T>(string key, T value, TimeSpan expiry, CancellationToken cancellationToken = default);
        Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
        Task SetListAsync<T>(string key, List<T> list, TimeSpan expiry, CancellationToken cancellationToken = default);
        Task<List<T>?> GetListAsync<T>(string key, CancellationToken cancellationToken = default);
        Task RemoveAsync(string key, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);
        Task<bool> ExpireAsync(string key, TimeSpan expiry, CancellationToken cancellationToken = default);
        Task<bool> AcquireLockAsync(string key, TimeSpan expiry);
        Task ReleaseLockAsync(string key);


    }
}
