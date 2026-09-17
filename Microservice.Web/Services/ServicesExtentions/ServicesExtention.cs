using Microservice.Web.Services.PageDetailResolvers;
using Microservice.Web.Services.PageResolvers;
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
            services.AddScoped<IBannerService, BannerService>();
            services.AddScoped<IBilgiService, BilgiService>();
            services.AddScoped<IEtkinlikService, EtkinlikService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<IDilService, DilService>();
            services.AddScoped<IShortcutButtonService, ShortcutButtonService>();
            services.AddScoped<IBilgiService, BilgiService>();
            services.AddScoped<IBandLogoService, BandLogoService>();
            services.AddScoped<IIcerikService, IcerikService>();
            services.AddScoped<IPageTypeService, PageTypeService>();
            services.AddScoped<ISitePersonelService, SitePersonelService>();
            services.AddScoped<IGaleriResimService, GaleriResimService>();
            services.AddScoped<IIletisimService, IletisimService>();

            services.AddScoped<IPageDetailResolver, HaberDetayResolver>();
            services.AddScoped<IPageDetailResolver, DuyuruDetayResolver>();
            services.AddScoped<IPageDetailResolver, MenuDetayResolver>();
            services.AddScoped<IPageDetailResolver, BilgiDetayResolver>();
            services.AddScoped<IPageDetailResolver, EtkinlikDetayResolver>();
            services.AddScoped<IPageDetailResolver, VideoDetayResolver>();
            services.AddScoped<IPageDetailResolver, PersonelDetayResolver>();
            services.AddScoped<IPageDetailResolver, GaleriResimDetayResolver>();

            services.AddScoped<IPageResolver, HaberListesiPageResolver>();
            services.AddScoped<IPageResolver, DuyuruListesiPageResolver>();
            services.AddScoped<IPageResolver, VideoListesiPageResolver>();
            services.AddScoped<IPageResolver, PersonelListesiPageResolver>();

            return services;
        }
    }
}
