using MassTransit;
using Microservice.Web.Settings;

namespace Microservice.Web.Messaging
{
    /// <summary>
    /// Microservice.Web icin MassTransit/RabbitMQ consumer yapilandirmasi.
    /// Publish edilen PersonelSyncCompletedEvent / OgrenciSyncCompletedEvent
    /// bu consumer tarafindan dinlenir.
    /// </summary>
    public static class RabbitmqConsumerExtension
    {
        public static IServiceCollection AddRabbitmqConsumerExtensions(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumers(typeof(RabbitmqConsumerExtension).Assembly);

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
