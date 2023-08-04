namespace BackendProjem.Caching.Core;

/// <summary>
/// Kullanılacak cache servisi bu interface'i implemente eder ve bunun üzerinden kullanılır.
/// </summary>
public interface ICacheManager
{
    /// <summary>
    /// Sonsuz süreyle cache'e atar.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    void Set(string key, string value, CacheRegion region);

    /// <summary>
    /// Sonsuz süreyle async olarak cache'e atar.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    Task SetAsync(string key, object value, CacheRegion region);

    /// <summary>
    /// Verilen süreye kadar cache'e atar.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expiration"></param>
    void Set(string key, object value, CacheRegion region, TimeSpan expiration);

    /// <summary>
    /// Verilen süreye kadar async olarak cache'e atar.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expiration"></param>
    Task SetAsync(string key, object value, CacheRegion region, TimeSpan expiration, CachePlatform cachePlatform);

    /// <summary>
    /// Cache'i kontrol eder, eğer cache'de veri varsa döndürür.
    /// Eğer yoksa, func methodunu çağırır ve sonucunu önce cache'e atar, sonra sonucu döndürür.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="key"></param>
    /// <param name="region"></param>
    /// <param name="platform"></param>
    /// <param name="expiration"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    Task<TValue> GetOrSetAsync<TValue>(string key, CacheRegion region, CachePlatform platform, TimeSpan expiration, Func<Task<TValue>> func) where TValue : class;

    /// <summary>
    /// Eğer bu key'e ait bir obje varsa getirir.
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    TValue Get<TValue>(string key, CacheRegion region, CachePlatform platform) where TValue : class;

    /// <summary>
    /// Eğer bu key'e ait bir obje varsa async olarak getirir.
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    Task<TValue> GetAsync<TValue>(string key, CacheRegion region, CachePlatform platform) where TValue : class;

    /// <summary>
    /// Cache'ten bu key'i siler.
    /// </summary>
    /// <param name="key"></param>
    void Remove(string key, CacheRegion region, CachePlatform platform = CachePlatform.Distributed);

    /// <summary>
    /// Cache'ten bu key'i async olarak siler.
    /// </summary>
    /// <param name="key"></param>
    Task RemoveAsync(string key, CacheRegion region, CachePlatform platform = CachePlatform.Distributed);
}
