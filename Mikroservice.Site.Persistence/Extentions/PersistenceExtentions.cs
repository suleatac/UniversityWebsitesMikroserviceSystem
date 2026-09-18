using Microservice.Site.Application.Contracts.IRepositories;
using Microservice.Site.Persistence.Repositories;
using Microservice.Site.Persistence.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microservice.Shared.Options;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Contracts.Services;
using Mikroservice.Site.Persistence;
using Mikroservice.Site.Persistence.Repositories;
using Mikroservice.Site.Persistence.Services;
using Mikroservice.Site.Persistence.Services.Nginx;
using Mikroservice.Site.Persistence.Settings;
using StackExchange.Redis;
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
            services.AddScoped<IGaleriResimRepository, GaleriResimRepository>();
            services.AddScoped<IHaberRepository, HaberRepository>();
            services.AddScoped<IHedefRepository, HedefRepository>();
            services.AddScoped<IPageTypeRepository, PageTypeRepository>();
            services.AddScoped<IIcerikRepository, IcerikRepository>();
            services.AddScoped<IIcerikDosyaRepository, IcerikDosyaRepository>();
            services.AddScoped<IIcerikResimRepository, IcerikResimRepository>();
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
            services.AddScoped<IShortcutButtonRepository, ShortcutButtonRepository>();

            // nginx yapılandırma üretimi.
            services.Configure<NginxConfigSetting>(
                configuration.GetSection(NginxConfigSetting.SectionName));

            services.AddScoped<SiteNginxConfigService>();

            // Dosya sistemi erişimi ve kilitleme durumsuzdur; singleton olarak tutulabilir.
            services.AddSingleton<INginxConfigStore, FileSystemNginxConfigStore>();

            // IConnectionMultiplexer yalnızca AddRedisCacheExt çağrıldığında kayıtlıdır.
            // GetService ile alınması, Redis kayıtlı değilse kilidin süreç içi moda
            // düşmesini sağlar; böylece bu servis farklı host'larda da çözülebilir.
            services.AddSingleton<INginxConfigLockProvider>(serviceProvider =>
                new RedisNginxConfigLockProvider(
                    serviceProvider.GetService<IConnectionMultiplexer>(),
                    serviceProvider.GetRequiredService<IOptions<NginxConfigSetting>>(),
                    serviceProvider.GetRequiredService<ILogger<RedisNginxConfigLockProvider>>()));

            // Reloader, ilk reload isteğinde arka plan işini kendisi (tembel olarak) başlatır;
            // EnableConfigReload=false iken hiç kaynak tüketmez. IHostedService olarak
            // kaydedilmesi gereksizdir, çünkü yapacağı iş yalnızca istek üzerine tetiklenir.
            services.AddSingleton<INginxReloader, NginxReloader>();

            // Şablon sürümü yükseltmelerini ve drift'i periyodik olarak onarır.
            services.AddHostedService<NginxConfigReconcileHostedService>();

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
