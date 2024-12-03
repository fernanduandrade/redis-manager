namespace RedisManagerApi.Contracts;

public sealed record RedisKey(Guid Id,
    string Name,
    RedisKeyType Type,
    int Count,
    string Parent,
    bool Expanded = false);

public enum RedisKeyType
{
    Key,
    KeySpace,
}