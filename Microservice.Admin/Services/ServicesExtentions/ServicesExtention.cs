using Microservice.Admin.Services.Interfaces;

namespace Microservice.Admin.Services.ServicesExtentions
{
    public static class ServicesExtention
    {
        public static IServiceCollection AddServicesExtentions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMinioService, MinioService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBirimService, BirimService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IRedisCacheService, RedisCacheService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISiteService, SiteService>();
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<IDilService, DilService>();
            services.AddScoped<IHedefService, HedefService>();
            services.AddScoped<IHaberService, HaberService>();
            services.AddScoped<IYonetimDuyuruService, YonetimDuyuruService>();
            return services;
        }
    }
}
