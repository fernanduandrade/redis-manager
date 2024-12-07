using RedisManagerApi.Contracts;
using RedisManagerApi.Shared;

namespace RedisManagerApi.Services;

public interface ICacheManagerService
{
    Task<Result<Guid, Error>> OpenConnectionAsync(string connectionString);
    Task OpenConnectionAsync(string connectionString, Guid id);
    Task<Result<List<RedisKey>, Error>> GetKeysAsync(Guid id, string keyspace);
    Task<Result<string, Error>> GetCacheKeyValue(Guid id, string cacheKey);
    Task UpdateKeyValue(Guid id, string cacheKey, string value);
    Task<Result<RedisKey, Error>> CreateKeyValue(Guid id, string cacheKey, string value);
}