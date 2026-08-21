using Microservice.Web.Services.Interfaces;

namespace Microservice.Web.Services.ServicesExtentions
{
    public static class ServicesExtention
    {
        public static IServiceCollection AddServicesExtentions(this IServiceCollection services, IConfiguration configuration)
        {
  
            services.AddScoped<IRedisCacheService, RedisCacheService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ISiteService, SiteService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IHaberService, HaberService>();
            services.AddScoped<IDuyuruService, DuyuruService>();
            services.AddScoped<IPageRouteService, PageRouteService>();
            return services;
        }
    }
}
