using RedisManagerApi.Contracts;
using RedisManagerApi.Shared;

namespace RedisManagerApi.Services;

public interface ICacheManagerService
{
    Task<Result<Guid, Error>> OpenConnectionAsync(string connectionString);
    Task<Result<List<RedisKey>, Error>> GetKeysAsync(RedisClientConnection clientConnection, string keyspace);
    Task<Result<string, Error>> GetCacheKeyValue(RedisClientConnection clientConnection, string cacheKey);
}