using Microservice.Web.Settings;
using Microsoft.Extensions.Options;

namespace Microservice.Web.Configurations
{
    public static class MicroservicesConfiguration
    {
        public static IServiceCollection AddMicroservicesConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddOptions<MicroservicesSetting>()
                           .BindConfiguration(MicroservicesSetting.SectionName)
                           .ValidateDataAnnotations()
                           .ValidateOnStart();
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<MicroservicesSetting>>().Value);
            return services;
        }
    }
}
