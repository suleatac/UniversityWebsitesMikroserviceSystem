using Microservice.Site.Application.Contracts.IRepositories;
using Microservice.Site.Persistence.Repositories;
using Microservice.Site.Persistence.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Contracts.Services;
using Mikroservice.Site.Persistence;
using Mikroservice.Site.Persistence.Repositories;
using Mikroservice.Site.Persistence.Services;
namespace Microservice.Site.Persistence.Extentions
{
    public static class PersistenceExtentions
    {
        public static IServiceCollection AddPersistenceExtentions(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<AppDbContext>(options => {
                var connectionToString = configuration.GetSection(ConnectionTostringOption.Key).Get<ConnectionTostringOption>();
                options.UseNpgsql(connectionToString!.PostgreSqlServer, sqlServerOptionAction => {
                    sqlServerOptionAction.MigrationsAssembly(typeof(PersistenceAssembly).Assembly.FullName);
                    sqlServerOptionAction.EnableRetryOnFailure(
                                                  maxRetryCount: 5,
                                                  maxRetryDelay: TimeSpan.FromSeconds(30),
                                                  errorCodesToAdd: null);
                });

            });


            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IBandLogoRepository, BandLogoRepository>();
            services.AddScoped<IBannerRepository, BannerRepository>();
            services.AddScoped<IBilgiRepository, BilgiRepository>();
            services.AddScoped<IBirimRepository, BirimRepository>();
            services.AddScoped<IDilRepository, DilRepository>();
            services.AddScoped<IDuyuruRepository, DuyuruRepository>();
            services.AddScoped<IEtkinlikRepository, EtkinlikRepository>();
            services.AddScoped<IHaberRepository, HaberRepository>();
            services.AddScoped<IHedefRepository, HedefRepository>();
            services.AddScoped<IIcerikRepository, IcerikRepository>();
            services.AddScoped<IMediaFileRepository, MediaFileRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IPersonelTelefonRepository, PersonelTelefonRepository>();
            services.AddScoped<IPersonelTipRepository, PersonelTipRepository>();
            services.AddScoped<IPopupRepository, PopupRepository>();
            services.AddScoped<ISikcaSorulanSoruRepository, SikcaSorulanSoruRepository>();
            services.AddScoped<ISiteOzellikleriRepository, SiteOzellikleriRepository>();
            services.AddScoped<ISitePersonelRepository, SitePersonelRepository>();
            services.AddScoped<ISiteRepository, SiteRepository>();
            services.AddScoped<ITemplateRepository, TemplateRepository>();
            services.AddScoped<IUnvanRepository, UnvanRepository>();
            services.AddScoped<IVideoRepository, VideoRepository>();
            services.AddScoped<IYoneticiSiteRepository, YoneticiSiteRepository>();
            services.AddScoped<IYonetimDuyuruRepository, YonetimDuyuruRepository>();
            services.AddScoped<IYonetimDuyuruOkunduRepository, YonetimDuyuruOkunduRepository>();

            
            services.AddScoped<ISeedService, UnvanSeedService>();
            services.AddScoped<ISeedService, PersonelTipSeedService>();
            services.AddScoped<ISeedService, HedefSeedService>();
            services.AddScoped<ISeedService, DilSeedService>();
            services.AddScoped<ISeedService, BirimSeedService>();
            services.AddScoped<ISeedService, TemplateSeedService>();

            
            services.AddScoped<IUserContextService, UserContextService>();




            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;

        }
    }
}
