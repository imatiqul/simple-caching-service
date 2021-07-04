using StackExchange.Redis;

namespace Simple.Caching.API.Interfaces
{
    public interface IAzureRedisCacheHandler
    {
        IDatabase GetDatabase();
    }
}
