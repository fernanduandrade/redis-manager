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
    
    public async Task<Result<List<RedisKeySpaces>, Error>> GetKeySpacesConnectionAsync(RedisClientConnection clientConnection)
    {
        ConnectionMultiplexer connectionMultiplexer = null;
        
        if (!cacheManager.CacheConnection.TryGetValue(clientConnection.Id,
                out ConnectionMultiplexer connMult))
        {
            string connectionString = $"{clientConnection.Host}:{clientConnection.Port}";
            var connection = await ConnectionMultiplexer.ConnectAsync(connectionString);
            cacheManager.CacheConnection[clientConnection.Id] = connection;
            connectionMultiplexer = connection;
        }
        else
        {
            connectionMultiplexer = connMult;
        }

        var server = connectionMultiplexer!.GetServer(connectionMultiplexer.GetEndPoints()[0]);
        var keys = server.Keys();
        if(!keys.Any())
            return new Error("KeySpaceNotFound", "There is no keyspaces for the connection to Redis");
        
        var namespaces = new HashSet<string>();

        List<RedisKeySpaces> keySpaces = new();
        foreach (var key in keys)
        {
            var keyString = key.ToString();
            var namespacePrefix = keyString.Contains(':') ? keyString.Split(':')[0] : "";
            if (!string.IsNullOrEmpty(namespacePrefix) && !namespaces.Contains(namespacePrefix))
            {
                namespaces.Add(namespacePrefix);
                int count = server.Keys(pattern: $"{namespacePrefix}:*").Count();
                keySpaces.Add(new RedisKeySpaces(namespacePrefix, count));
            }
        }
        
        return keySpaces;
    }
    
    public async Task<Result<List<RedisKey>, Error>> GetKeysAsync(RedisClientConnection clientConnection, string keyspace)
    {
        if (!cacheManager.CacheConnection.TryGetValue(clientConnection.Id,
                out ConnectionMultiplexer connectionMultiplexer))
        {
            string connectionString = $"{clientConnection.Host}:{clientConnection.Port}";
            var connection = await ConnectionMultiplexer.ConnectAsync(connectionString);
            cacheManager.CacheConnection[clientConnection.Id] = connection;
        }

        var server = connectionMultiplexer!.GetServer(connectionMultiplexer.GetEndPoints()[0]);
        var keys = server.Keys(pattern: $"{keyspace}:*");
        
        var hashKeys = new HashSet<string>();

        List<RedisKey> keysList = new();
        foreach (var key in keys)
        {
            var keyString = key.ToString()!;
            var keyValue = keyString.Split(":")[1];
            var hasChildren = server.Keys(pattern: $"{keyspace}:{keyValue}:*");
            if(hashKeys.Contains(keyString))
                continue;
            
            var isKeySpace = hasChildren.Any();
            hashKeys.Add(keyValue);
            keysList.Add(new RedisKey(Guid.NewGuid(), keyValue, isKeySpace, hasChildren.Count()));
        }
        
        return keysList;
    }
    
    public async Task<Result<string, Error>> GetCacheKeyValue(RedisClientConnection clientConnection, string cacheKey)
    {
        if (!cacheManager.CacheConnection.TryGetValue(clientConnection.Id,
                out ConnectionMultiplexer connectionMultiplexer))
        {
            string connectionString = $"{clientConnection.Host}:{clientConnection.Port}";
            var connection = await ConnectionMultiplexer.ConnectAsync(connectionString);
            cacheManager.CacheConnection[clientConnection.Id] = connection;
        }

        string value= await connectionMultiplexer.GetDatabase().StringGetAsync(cacheKey);
        return value!;
    }
}