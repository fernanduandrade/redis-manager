namespace RedisManagerApi.Contracts;

public sealed record RedisKey(Guid Id, string Name, bool IsKeySpace, int Count);