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
        

        var server = connectionMultiplexer!.GetServer(connectionMultiplexer.GetEndPoints()[0]);
        var keys = string.IsNullOrEmpty(keyspace)
            ? server.Keys()
            : server.Keys(pattern: $"{keyspace}:*");
        
        var hashKeys = new HashSet<string>();

        List<RedisKey> keysList = new();
        foreach (var key in keys)
        {
            var keyString = key.ToString()!;
            var keyValue = string.IsNullOrEmpty(keyspace) ?
                keyString: keyString.Split(":")[1];

            string search = string.IsNullOrEmpty(keyspace) ? keyString : $"{keyspace}:{keyValue}";  
            var hasChildren = server.Keys(pattern: search); 
            if(hashKeys.Contains(keyString))
                continue;

            string parent = $"{keyspace}";
            var type = hasChildren.Any() ? RedisKeyType.KeySpace : RedisKeyType.Key;
            hashKeys.Add(keyValue);
            keysList.Add(new RedisKey(Guid.NewGuid(), keyValue, type, hasChildren.Count(), parent));
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
}