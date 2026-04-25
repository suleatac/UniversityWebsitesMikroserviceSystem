using Microservice.Admin.Settings;
using Microsoft.Extensions.Options;
using Minio;

namespace Microservice.Admin.Configurations
{
    public static class MinioConfiguration
    {
        public static IServiceCollection AddMinioExtentions(this IServiceCollection services, IConfiguration configuration)
        {

            //Minio Client'ı yapılandırma
            services.Configure<MinioSetting>(
                configuration.GetSection("Minio"));

            services.AddSingleton(sp => {
                var settings = sp.GetRequiredService<IOptions<MinioSetting>>().Value;

                return new MinioClient()
                    .WithEndpoint(settings.Endpoint)
                    .WithCredentials(settings.Username, settings.Password)
                    .WithSSL(settings.UseSSL)
                    .Build();
            });

            return services;
        }
    }
}
