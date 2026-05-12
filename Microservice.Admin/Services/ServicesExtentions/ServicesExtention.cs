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
            services.AddScoped<IUnvanService, UnvanService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IPersonelTipService, PersonelTipService>();
            services.AddScoped<IDuyuruService, DuyuruService>();
            services.AddScoped<IEtkinlikService, EtkinlikService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<ISikcaSorulanSoruService, SikcaSorulanSoruService>();
            services.AddScoped<ISikcaSorulanSoruKategoriService, SikcaSorulanSoruKategoriService>();
            services.AddScoped<ISitePersonelService, SitePersonelService>();
            services.AddScoped<IBilgiService, BilgiService>();
            services.AddScoped<IBannerService, BannerService>();
            services.AddScoped<IYoneticiTipiService, YoneticiTipiService>();
            services.AddScoped<IYoneticiSiteService, YoneticiSiteService>();
            services.AddScoped<IKeycloakRoleService, KeycloakRoleService>();
            services.AddScoped<IPopupService, PopupService>();
            services.AddScoped<ISiteOzellikleriService, SiteOzellikleriService>();
            services.AddScoped<ITumPersonelService, TumPersonelService>();
            return services;
        }
    }
}
