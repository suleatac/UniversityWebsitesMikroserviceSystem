using Microservice.Web.Settings;
using Microsoft.Extensions.Options;

namespace Microservice.Web.Configurations
{
    public static class IdentityServerConfiguration
    {
        public static IServiceCollection AddIdentityServerExtentions(this IServiceCollection services, IConfiguration configuration)
        {

            //Identity Server Configuration
            services.AddOptions<IdentitySetting>()
                          .BindConfiguration(IdentitySetting.SectionName)
                          .ValidateDataAnnotations()
                          .ValidateOnStart();

            services.AddSingleton(sp => sp.GetRequiredService<IOptions<IdentitySetting>>().Value);

            return services;
        }
    }
}
