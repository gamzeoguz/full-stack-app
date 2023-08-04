using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace BackendProjem.Caching.Core;

public class CacheManager : ICacheManager
{
    private readonly IDistributedCache _distributedCache;
    private readonly IMemoryCache _memoryCache;
    private readonly string environmentName;

    public CacheManager(IDistributedCache distributedCache, IMemoryCache memoryCache, IConfiguration configuration)
    {
        _distributedCache = distributedCache;
        _memoryCache = memoryCache;
        environmentName = configuration.GetSection("ApplicationNameRedisCache")?.Value ?? string.Empty;
    }

    public TValue Get<TValue>(string key, CacheRegion region, CachePlatform platform) where TValue : class
    {
        return platform == CachePlatform.Distributed
            ? _distributedCache.Get<TValue>(SetPrefix(key), region)
            : _memoryCache.Get<TValue>(key);
    }

    public async Task<TValue> GetAsync<TValue>(string key, CacheRegion region, CachePlatform platform)
        where TValue : class
    {
        return platform == CachePlatform.Distributed
            ? await _distributedCache.GetAsync<TValue>(SetPrefix(key), region)
            : _memoryCache.Get<TValue>(key);
    }

    public void Remove(string key, CacheRegion region, CachePlatform platform = CachePlatform.Distributed)
    {
        if (platform == CachePlatform.InMemory)
        {
            _memoryCache.Remove(key);
            return;
        }

        _distributedCache.Remove(SetPrefix(key), region);
    }

    public async Task RemoveAsync(string key, CacheRegion region, CachePlatform platform = CachePlatform.Distributed)
    {
        if (platform == CachePlatform.InMemory)
        {
            _memoryCache.Remove(key);
            return;
        }

        await _distributedCache.RemoveAsync(SetPrefix(key), region);
    }

    public void Set(string key, string value, CacheRegion region)
    {
        _distributedCache.Set(SetPrefix(key), value, region);
    }

    public void Set(string key, object value, CacheRegion region, TimeSpan expiration)
    {
        _distributedCache.Set(SetPrefix(key), value, region, expiration);
    }

    public async Task SetAsync(string key, object value, CacheRegion region)
    {
        await _distributedCache.SetAsync(SetPrefix(key), value, region);
    }

    public async Task SetAsync(string key, object value, CacheRegion region, TimeSpan expiration, CachePlatform cachePlatform)
    {
        
        if (cachePlatform == CachePlatform.Distributed)
        {
            await _distributedCache.SetAsync(SetPrefix(key), value, region, expiration);
            return; 
        }
        _memoryCache.Set(key, value, expiration);
    }

    public async Task<TValue> GetOrSetAsync<TValue>(string key, CacheRegion region, CachePlatform platform,
        TimeSpan expiration, Func<Task<TValue>> func) where TValue : class
    {
        var value = platform == CachePlatform.Distributed
            ? await _distributedCache.GetAsync<TValue>(SetPrefix(key), region)
            : _memoryCache.Get<TValue>(key);

        if (value != null)
        {
            return value;
        }

        value = await func();
        if (value == null)
        {
            return value;
        }

        if (CachePlatform.Distributed == platform)
        {
            await _distributedCache.SetAsync(SetPrefix(key), value, region, expiration);
            return value;
        }

        _memoryCache.Set(key, value, expiration);
        return value;
    }

    /// <summary>
    /// Redis'te tutulan Key'lerin başına environment bilgisini ekler.
    /// </summary>
    /// <param name="key">Parametre olarak Key bilgisini alır.</param>
    /// <returns>Return Key Info</returns>
    private string SetPrefix(string key)
    {
        return $"{environmentName}{key}";
    }
}