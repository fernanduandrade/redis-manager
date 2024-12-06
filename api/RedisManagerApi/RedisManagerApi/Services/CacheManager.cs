using RedisManagerApi.Contracts;
using RedisManagerApi.Infra;
using RedisManagerApi.Shared;
using StackExchange.Redis;
using RedisKey = RedisManagerApi.Contracts.RedisKey;

namespace RedisManagerApi.Services;

public class CacheManagerService(CacheManager cacheManager) : ICacheManagerService
{
    public async Task<Result<Guid, Error>> OpenConnectionAsync(string connectionString)
    {
        try
        {
            var connection = await ConnectionMultiplexer.ConnectAsync(connectionString);
            if (connection.IsConnected)
            {
                var identifier = Guid.NewGuid();
                if (!cacheManager.CacheConnection.TryGetValue(identifier,
                        out ConnectionMultiplexer connectionMultiplexer))
                {
                    cacheManager.CacheConnection[identifier] = connection;
                }
                return identifier;
            }
            else
                return new Error("ConnectionFailed", "Failed to establish a connection to Redis");       
        }
        catch (Exception ex)
        {
            return new Error("ConnectionFailed", "It Wasn't possible to create a connection to Redis, please verify the credentials");
        }
    }
    
    public async Task OpenConnectionAsync(string connectionString, Guid id)
    {
        try
        {
            if (!cacheManager.CacheConnection.TryGetValue(id,
                    out ConnectionMultiplexer connMult))
            {
                var connection = await ConnectionMultiplexer.ConnectAsync(connectionString);
                cacheManager.CacheConnection[id] = connection;
            }

        }
        catch (Exception ex)
        {
            throw new  Exception("ConnectionFailed", ex);
        }
    }
    
    public async Task<Result<List<RedisKey>, Error>> GetKeysAsync(Guid id, string keyspace)
    {
        if (string.IsNullOrEmpty(keyspace))
            return await GetKeys(id);
        
        cacheManager.CacheConnection.TryGetValue(id,
            out ConnectionMultiplexer connectionMultiplexer);

        string searchPattern = $"{keyspace}:*";
        var server = connectionMultiplexer!.GetServer(connectionMultiplexer.GetEndPoints()[0]);
        var keysFound = server.Keys(pattern: searchPattern).ToList();
        var filteredKeys = FilterResult(keysFound, searchPattern);
        List<RedisKey> keysList = new();
        foreach (var key in filteredKeys)
        {
            var keyString = key.ToString()!;
            var keyValue = !keyspace.Contains(":")
                    ? keyString.Split(":")[1]
                    : keyString.Split(":")[^1];

            string search = $"{keyspace}:{keyValue}:*";
            var hasChildren = server.Keys(pattern: search);

            string parent = $"{keyspace}";
            var type = hasChildren.Any() ? RedisKeyType.KeySpace : RedisKeyType.Key;
            keysList.Add(new RedisKey(Guid.NewGuid(), keyValue, type, filteredKeys.Count, parent));
        }
        
        return keysList;
    }
    
    private async Task<Result<List<RedisKey>, Error>> GetKeys(Guid id)
    {
        cacheManager.CacheConnection.TryGetValue(id,
            out ConnectionMultiplexer connectionMultiplexer);
        

        var server = connectionMultiplexer!.GetServer(connectionMultiplexer.GetEndPoints()[0]);
        var keys = server.Keys();
        if(!keys.Any())
            return new Error("KeySpaceNotFound", "There is no keyspaces for the connection to Redis");
        
        var namespaces = new HashSet<string>();

        List<RedisKey> keysList = new();
        foreach (var key in keys)
        {
            var keyString = key.ToString();
            var namespacePrefix = keyString.Contains(':') ? keyString.Split(':')[0] : keyString;
            var type =  keyString.Contains(':') ? RedisKeyType.KeySpace : RedisKeyType.Key;
            if (!namespaces.Contains(namespacePrefix))
            {
                namespaces.Add(namespacePrefix);
                int count = server.Keys(pattern: $"{namespacePrefix}:*").Count();
                keysList.Add(new RedisKey(Guid.NewGuid(), namespacePrefix, type, count, ""));
            }
        }
        
        return keysList;
    }
    
    public async Task<Result<string, Error>> GetCacheKeyValue(Guid id, string cacheKey)
    {
        cacheManager.CacheConnection.TryGetValue(id,
            out ConnectionMultiplexer connectionMultiplexer);
        string value= await connectionMultiplexer.GetDatabase().StringGetAsync(cacheKey);
        return value!;
    }
    
    public async Task UpdateKeyValue(Guid id, string cacheKey, string value)
    {
        cacheManager.CacheConnection.TryGetValue(id,
            out ConnectionMultiplexer connectionMultiplexer);

        var db =  connectionMultiplexer.GetDatabase();
        await  db.StringSetAsync(cacheKey, value);
    }

    private List<StackExchange.Redis.RedisKey> FilterResult(List<StackExchange.Redis.RedisKey> results, string searchPattern)
    {var basePattern = searchPattern.TrimEnd('*');

        return results
            .Where(result => result.ToString().StartsWith(basePattern))
            .Select(result =>
            {
                var remaining = result.ToString().Substring(basePattern.Length);
                var nextColonIndex = remaining.IndexOf(':');
                return nextColonIndex != -1
                    ? (StackExchange.Redis.RedisKey)(basePattern + remaining.Substring(0, nextColonIndex))
                    : (StackExchange.Redis.RedisKey)(basePattern + remaining);
            })
            .Distinct()
            .ToList();
    }
}