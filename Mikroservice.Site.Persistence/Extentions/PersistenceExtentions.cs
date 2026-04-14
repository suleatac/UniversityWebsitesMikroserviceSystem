using Microservice.Site.Application.Contracts.IRepositories;
using Microservice.Site.Application.Contracts.Services;
using Microservice.Site.Persistence;
using Microservice.Site.Persistence.Repositories;
using Microservice.Site.Persistence.Services;
using Microservice.Site.Persistence.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mikroservice.Site.Persistence;
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
            services.AddScoped<IYoneticiTipiRepository, YoneticiTipiRepository>();
            services.AddScoped<IYoneticiDuyuruRepository, YoneticiDuyuruRepository>();
            services.AddScoped<IYoneticiTipiSeedService, YoneticiTipiSeedService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;

        }
    }
}

