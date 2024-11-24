using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using RedisManagerApi.Contracts;
using RedisManagerApi.Services;

namespace RedisManagerApi.Handlers;

public sealed record ConnectionRequestDto(string Host, int Port, string UserName, string Password);
public sealed record KeySpaceConnectionRequestDto(
    [property: JsonPropertyName("id")] Guid identifier,
    [property: JsonPropertyName("connection")] RedisConnection Connection);

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
    
    internal static async Task<IResult> GetConnectionKeySpaces(Guid id, string host, string port, string? username, string? password,
        [FromServices] ICacheManagerService cacheManagerService)
    {
        var dto  = new RedisClientConnection(
            id,
            host,
            port,
            username,
            password);
        var result = await cacheManagerService.GetKeySpacesConnectionAsync(dto);
        if (result.IsSuccess)
            return TypedResults.Ok(result.Value);
        
        return TypedResults.BadRequest(result.Error);
    }
}