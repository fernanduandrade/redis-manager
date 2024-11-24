using RedisManagerApi.Contracts;
using RedisManagerApi.Shared;

namespace RedisManagerApi.Services;

public interface ICacheManagerService
{
    Task<Result<Guid, Error>> OpenConnectionAsync(string connectionString);
    Task<Result<List<RedisKeySpaces>, Error>> GetKeySpacesConnectionAsync(RedisClientConnection clientConnection);
}