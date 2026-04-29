using MassTransit;
using Microservice.Shared.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mikroservice.Site.Persistence.Messaging.Consumers.BandLogoConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.BannerConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.BilgiConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.BirimConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.DuyuruConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.EtkinlikConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.HaberConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.MediaFileConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.MenuConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.PersonelTipConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.PopupConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.SertifikaParmakIziConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.SikcaSorulanSoruConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.SikcaSorulanSoruKategoriConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.SiteConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.SiteOzellikleriConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.SitePersonelConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.TemplateConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.UnvanConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.VideoConsumer;
using Mikroservice.Site.Persistence.Messaging.Consumers.YoneticiSiteConsumers;

namespace Mikroservice.Site.Persistence.Messaging.RabbitmqExtentions
{
    public static class RabbitmqExtention
    {
        public static IServiceCollection AddRabbitmqExtentions(this IServiceCollection services, IConfiguration configuration)
        {
  
            services.AddMassTransit(x =>
            {
                x.AddConsumers(typeof(TemplateChangedEventConsumer).Assembly);

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
