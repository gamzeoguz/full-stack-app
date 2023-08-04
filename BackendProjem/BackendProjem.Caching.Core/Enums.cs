namespace BackendProjem.Caching.Core
{
    public enum CachePlatform
    {
        Distributed = 1,
        InMemory = 2
    }

    public enum CacheRegion
    {
        Default = 0,
        Session = 1
    }

    public enum ActionType
    {
        Set = 1,
        Remove = 2
    }
}
