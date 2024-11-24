using System.Collections.Concurrent;
using StackExchange.Redis;

namespace RedisManagerApi.Infra;

public class CacheManager
{
    private readonly ConcurrentDictionary<Guid, ConnectionMultiplexer> _cacheConnection = new();
    public ConcurrentDictionary<Guid, ConnectionMultiplexer> CacheConnection => _cacheConnection;
}