namespace Microservice.Shared.Services.RedisServiceItems
{
    public interface IRedisCacheService
    {
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default);
        Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
        Task SetListAsync<T>(string key, List<T> list, TimeSpan? expiry = null, CancellationToken cancellationToken = default);
        Task<List<T>?> GetListAsync<T>(string key, CancellationToken cancellationToken = default);
        Task RemoveAsync(string key, CancellationToken cancellationToken = default);
        Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);
        Task<bool> ExpireAsync(string key, TimeSpan expiry, CancellationToken cancellationToken = default);
    }
}
