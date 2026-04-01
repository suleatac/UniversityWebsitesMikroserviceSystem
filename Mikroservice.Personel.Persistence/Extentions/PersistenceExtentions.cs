using Microservice.Personel.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mikroservice.Personel.Persistence.Repositories;
using Mikroservice.Personel.Persistence.Settings;

namespace Mikroservice.Personel.Persistence.Extentions
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
            services.AddScoped<IPersonelRepository, PersonelRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.Configure<ExternalPersonelApiSettings>(configuration.GetSection("ExternalPersonelApiSettings"));
            return services;

        }
    }
}
