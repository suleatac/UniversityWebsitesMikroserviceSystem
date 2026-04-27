using Microservice.Admin.Services;
using StackExchange.Redis;
using System.Text.Json;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _db;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    // 🔹 Tek veri ekle
    public async Task SetAsync<T>(string key, T value, TimeSpan expiry, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var json = JsonSerializer.Serialize(value);
        await _db.StringSetAsync(key, json, expiry);
    }

    // 🔹 Tek veri getir
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var value = await _db.StringGetAsync(key);

        if (value.IsNullOrEmpty)
            return default;

        return JsonSerializer.Deserialize<T>(value!.ToString());
    }

    // 🔹 Liste ekle
    public async Task SetListAsync<T>(string key, List<T> list, TimeSpan expiry , CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var json = JsonSerializer.Serialize(list);
        await _db.StringSetAsync(key, json, expiry);
    }

    // 🔹 Liste getir
    public async Task<List<T>?> GetListAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var value = await _db.StringGetAsync(key);

        if (value.IsNullOrEmpty)
            return null;

        return JsonSerializer.Deserialize<List<T>>(value!.ToString());
    }

    // 🔹 Sil
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await _db.KeyDeleteAsync(key);
    }

    // 🔹 Var mı kontrolü
    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await _db.KeyExistsAsync(key);
    }

    // 🔹 Expire set et
    public async Task<bool> ExpireAsync(string key, TimeSpan expiry, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await _db.KeyExpireAsync(key, expiry);
    }



    public async Task<bool> AcquireLockAsync(string key, TimeSpan expiry)
    {
        return await _db.StringSetAsync(
            key,
            "1",
            expiry,
            When.NotExists // 🔥 SETNX
        );
    }

    public async Task ReleaseLockAsync(string key)
    {
        await _db.KeyDeleteAsync(key);
    }



}