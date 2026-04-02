using Microservice.Ogrenci.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mikroservice.Ogrenci.Infrastructure.Persistence.UnitOfWorks;
using Mikroservice.Ogrenci.Persistence.Repositories;
using Mikroservice.Ogrenci.Persistence.Settings;

namespace Mikroservice.Ogrenci.Persistence.Extentions
{
    public static class PersistenceExtentions
    {
        public static IServiceCollection AddPersistenceExtentions(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<AppDbContext>(options => {
                var connectionToString = configuration.GetSection(ConnectionTostringOption.Key).Get<ConnectionTostringOption>();
                options.UseNpgsql(connectionToString!.PostgreSqlServer, sqlServerOptionAction => {
                    sqlServerOptionAction.MigrationsAssembly(typeof(PersistenceAssembly).Assembly.FullName);
                    });

            });
            services.AddScoped<IOgrenciRepository, OgrenciRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.Configure<ExternalOgrenciApiSettings>(configuration.GetSection("ExternalOgrenciApiSettings"));
            return services;

        }
    }
}
