using Microservice.Admin.Services.Interfaces;

namespace Microservice.Admin.Services.ServicesExtentions
{
    public static class ServicesExtention
    {
        public static IServiceCollection AddServicesExtentions(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IRedisCacheService, RedisCacheService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISiteService, SiteService>();
            return services;
        }
    }
}
