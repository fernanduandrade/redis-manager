namespace RedisManagerApi.Contracts;

public sealed record RedisConnection(string Host, int Port, string UserName, string Password);
public sealed record RedisClientConnection(Guid Id, string Host, string Port, string UserName, string Password);