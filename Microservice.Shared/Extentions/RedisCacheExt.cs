using Microservice.Shared.RedisCacheItems;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Microservice.Shared.Extentions
{
    public static class RedisCacheExt
    {
        public static IServiceCollection AddRedisCacheExt(this IServiceCollection services, IConfiguration configuration)
        {

            // Redis Bağlantı ayarları. Bu ayar trace için yapıldı. IDistributedCache kullanmıyorsun burda direk redis kullanıyorsun.
            services.AddSingleton<IConnectionMultiplexer>(sp => {
                var connectiontostring = configuration.GetSection(RedisConnectionTostringOptions.Key).Get<RedisConnectionTostringOptions>();
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
