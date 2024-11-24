using RedisManagerApi.Services;

namespace RedisManagerApi.Configurations;

public static class ServicesDIConfig
{
    public static void InjectServices(this IServiceCollection services)
    {
        services.AddScoped<ICacheManagerService, CacheManagerService>();
    }
}