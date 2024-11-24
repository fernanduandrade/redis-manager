using RedisManagerApi.Infra;

namespace RedisManagerApi.Configurations;

public static class InfraDIConfig
{
    public static void InjectInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<CacheManager>();
    }
}