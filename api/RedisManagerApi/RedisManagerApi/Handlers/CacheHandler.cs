using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using RedisManagerApi.Contracts;
using RedisManagerApi.Services;

namespace RedisManagerApi.Handlers;

public sealed record ConnectionRequestDto(string Host, int Port, string UserName, string Password);
public sealed record UpdateKeyValueRequestDto(string Value);
public sealed record CreateKeyValueRequestDto(string Value, string Key);

public static class CacheHandler
{
    internal static async Task<IResult> CreateConnection([FromBody] ConnectionRequestDto request,
        [FromServices] ICacheManagerService cacheManagerService)
    {
        string fullHost = $"{request.Host}:{request.Port}";
        string connection = $"{fullHost}";
        var result = await cacheManagerService.OpenConnectionAsync(connection);
        if (result.IsSuccess)
            return TypedResults.Ok(result.Value);
        
        return TypedResults.BadRequest(result.Error);
    }
    
    internal static async Task<IResult> OpenConnection(Guid connectionId, string port, string host,
        [FromServices] ICacheManagerService cacheManagerService)
    {
        string fullHost = $"{host}:{port}";
        string connection = $"{fullHost}";
        await cacheManagerService.OpenConnectionAsync(connection, connectionId);
        
        return TypedResults.NoContent();
    }
    
    internal static async Task<IResult> GetKeys(Guid connectionId, string? pattern,
        [FromServices] ICacheManagerService cacheManagerService)
    {
        var result = await cacheManagerService.GetKeysAsync(connectionId, pattern);
        if (result.IsSuccess)
            return TypedResults.Ok(result.Value);
        
        return TypedResults.BadRequest(result.Error);
    }
    
    internal static async Task<IResult> GetCacheValue(Guid connectionId, [FromRoute] string hash,
        [FromServices] ICacheManagerService cacheManagerService)
    {
        var result = await cacheManagerService.GetCacheKeyValue(connectionId, hash);
        if (result.IsSuccess)
            return TypedResults.Ok(result.Value);
        
        return TypedResults.BadRequest(result.Error);
    }
    
    internal static async Task<IResult> UpdateKeyValue(Guid connectionId, [FromRoute] string hash, [FromBody] UpdateKeyValueRequestDto request,
        [FromServices] ICacheManagerService cacheManagerService)
    {
        await cacheManagerService.UpdateKeyValue(connectionId, hash, request.Value);

        return TypedResults.NoContent();
    }
    
    internal static async Task<IResult> CreateKeyValue(Guid connectionId, [FromBody] CreateKeyValueRequestDto request,
        [FromServices] ICacheManagerService cacheManagerService)
    {
        var result = await cacheManagerService.CreateKeyValue(connectionId, request.Key, request.Value);
        if (result.IsSuccess)
            return TypedResults.Ok(result.Value);
        
        return TypedResults.BadRequest(result.Error);
    }
}