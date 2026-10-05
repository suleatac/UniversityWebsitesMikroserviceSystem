using MassTransit;
using Microservice.Shared.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice.Shared.Extentions
{
    /// <summary>
    /// Publish-only MassTransit bus yapilandirmasi.
    /// Consumer'i olmayan servisler (orn. Personel/Ogrenci Api'deki Hangfire job'lari)
    /// event yayinlamak icin bu extension'i kullanir.
    /// </summary>
    public static class MassTransitPublishExt
    {
        public static IServiceCollection AddMassTransitPublishExt(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    var busOptions = configuration
                        .GetSection(nameof(RabbitMqMassTransitBusOption))
                        .Get<RabbitMqMassTransitBusOption>();

                    cfg.Host(new Uri($"rabbitmq://{busOptions!.Address}:{busOptions.Port}"), h =>
                    {
                        h.Username(busOptions.UserName);
                        h.Password(busOptions.Password);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
