using MessagePack;

namespace BackendProjem.Caching.Core;

[Serializable]
[MessagePackObject]
public class CacheMetaData
{
    public CacheMetaData(string key, CacheRegion region, ActionType action)
    {
        Key = key;
        Region = region;
        Action = action;
    }

    public CacheMetaData(string key, CacheRegion region, ActionType action, TimeSpan expiration)
    {
        Key = key;
        Region = region;
        Action = action;
        Expiration = expiration;
    }

    [Key(0)]
    public string Key { get; set; }

    [Key(1)]
    public CacheRegion Region { get; set; }

    [Key(2)]
    public ActionType Action { get; set; }

    [Key(3)]
    public TimeSpan Expiration { get; set; }
}
