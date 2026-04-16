using MassTransit;
using Microservice.Shared.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mikroservice.Site.Persistence.Messaging.Consumers;

namespace Mikroservice.Site.Persistence.Messaging.RabbitmqExtentions
{
    public static class RabbitmqExtention
    {
        public static IServiceCollection AddRabbitmqExtentions(this IServiceCollection services, IConfiguration configuration)
        {

            //RabbitMq Banner Delete Event Consumer
            services.AddMassTransit(x => {
                // 👇 Consumer’ları burada eklenmeli
                x.AddConsumer<BannerDeletedEventConsumer>();
                x.AddConsumer<BandLogoDeletedEventConsumer>();


                x.UsingRabbitMq((context, cfg) => {
                    var busOptions = configuration
                        .GetSection(nameof(RabbitMqMassTransitBusOption))
                        .Get<RabbitMqMassTransitBusOption>();

                    cfg.Host(new Uri($"rabbitmq://{busOptions!.Address}:{busOptions.Port}"), h => {
                        h.Username(busOptions.UserName);
                        h.Password(busOptions.Password);
                    });


                    // 👇 otomatik endpoint binding
                    cfg.ConfigureEndpoints(context);
                });
            });


            return services;
        }
    }
}
