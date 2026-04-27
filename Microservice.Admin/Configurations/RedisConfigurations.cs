using Microservice.Admin.Services;
using Microservice.Admin.Settings;
using StackExchange.Redis;

namespace Microservice.Admin.Configurations
{
    public static class RedisConfigurations
    {
        public static IServiceCollection AddRedisExtentions(this IServiceCollection services, IConfiguration configuration)
        {

            // Redis Bağlantı ayarları. Bu ayar trace için yapıldı. IDistributedCache kullanmıyorsun burda direk redis kullanıyorsun.
            services.AddSingleton<IConnectionMultiplexer>(sp => {
                var connectiontostring = configuration.GetSection(RedisSetting.Key).Get<RedisSetting>();
                var config = ConfigurationOptions.Parse(connectiontostring!.Redis);
                config.AbortOnConnectFail = false;

                var multiplexer = ConnectionMultiplexer.Connect(config);

                // 🔴 KRİTİK: instrumentation bunu otomatik yakalamaz
                return multiplexer;
            });

            services.AddScoped<IRedisCacheService, RedisCacheService>();



            return services;
        }

    }
}
