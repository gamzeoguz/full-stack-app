using BackendProjem.Caching.Core;
using BackendProjem.Serialization.Core;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace BackendProjem.Caching.Redis;

public class RedisCacheManager : IDistributedCache
{
    private readonly Lazy<ConnectionMultiplexer> _client;
    private readonly IByteSerializer _serializer;

    public RedisCacheManager(IConfiguration configuration, IByteSerializer serializer)
    {
        _serializer = serializer;

        var connectionString = configuration.GetSection("RedisConfiguration:ConnectionString")?.Value;
        _client = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString));
    }

    public RedisCacheManager(IByteSerializer serializer, string connectionString)
    {
        _serializer = serializer;

        _client = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString));
    }

    public TValue Get<TValue>(string key, CacheRegion region) where TValue : class
    {
        byte[] value = _client.Value.GetDatabase((int)region).StringGet(key);

        if (value == null)
            return null;

        return _serializer.Deserialize<TValue>(value);
    }

    public async Task<object> GetAsync(string key, CacheRegion region)
    {
        byte[] value = await _client.Value.GetDatabase((int)region).StringGetAsync(key);

        if (value == null)
        {
            return await Task.FromResult(default(object));
        }

        return value;
    }

    public async Task<TValue> GetAsync<TValue>(string key, CacheRegion region)
    {
        byte[] value = await _client.Value.GetDatabase((int)region).StringGetAsync(key);

        if (value == null)
        {
            return await Task.FromResult(default(TValue));
        }

        return _serializer.Deserialize<TValue>(value);
    }

    public void Set(string key, string value, CacheRegion region)
    {
        _client.Value.GetDatabase((int)region).StringSet(key, value);
    }

    public void Set(string key, object value, CacheRegion region, TimeSpan expiration)
    {
        _client.Value.GetDatabase((int)region).StringSet(key, _serializer.Serialize(value), expiration);
    }

    public Task SetAsync(string key, object value, CacheRegion region)
    {
        return _client.Value.GetDatabase((int)region).StringSetAsync(key, _serializer.Serialize(value));
    }

    public Task SetAsync(string key, object value, CacheRegion region, TimeSpan expiration)
    {
        return _client.Value.GetDatabase((int)region).StringSetAsync(key, _serializer.Serialize(value), expiration);
    }

    public void Remove(string key, CacheRegion region)
    {
        _client.Value.GetDatabase((int)region).KeyDelete(key);
    }

    public Task RemoveAsync(string key, CacheRegion region)
    {
        return _client.Value.GetDatabase((int)region).KeyDeleteAsync(key);
    }
}