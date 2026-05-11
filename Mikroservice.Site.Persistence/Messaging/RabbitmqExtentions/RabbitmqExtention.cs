using MassTransit;
using Microservice.Shared.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mikroservice.Site.Persistence.Messaging.RabbitmqExtentions
{
    public static class RabbitmqExtention
    {
        public static IServiceCollection AddRabbitmqExtentions(this IServiceCollection services, IConfiguration configuration)
        {
  
            services.AddMassTransit(x =>
            {
                x.AddConsumers(typeof(PersistenceAssembly).Assembly);

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
