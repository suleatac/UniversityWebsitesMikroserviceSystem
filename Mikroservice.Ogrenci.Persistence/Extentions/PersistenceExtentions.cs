using Microservice.Ogrenci.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mikroservice.Ogrenci.Persistence.Repositories;

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
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            return services;

        }
    }
}
