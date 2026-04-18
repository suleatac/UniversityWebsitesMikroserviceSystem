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
using Mikroservice.Site.Persistence.Messaging.Consumers.MenuConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.PersonelTipConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.PopupConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.SertifikaParmakIziConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.SikcaSorulanSoruConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.SikcaSorulanSoruKategoriConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.SiteConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.SiteOzellikleriConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.SitePersonelConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.UnvanConsumers;
using Mikroservice.Site.Persistence.Messaging.Consumers.VideoConsumer;
using Mikroservice.Site.Persistence.Messaging.Consumers.YoneticiSiteConsumers;

namespace Mikroservice.Site.Persistence.Messaging.RabbitmqExtentions
{
    public static class RabbitmqExtention
    {
        public static IServiceCollection AddRabbitmqExtentions(this IServiceCollection services, IConfiguration configuration)
        {

            //RabbitMq Banner Delete Event Consumer
            services.AddMassTransit(x => {
                // 👇 Consumer’ları burada eklenmeli


                //BandLogo Consumers
                x.AddConsumer<BandLogoCreatedEventConsumer>();
                x.AddConsumer<BandLogoDeletedEventConsumer>();
                x.AddConsumer<BandLogoUpdatedEventConsumer>();

                //Banner Consumers
                x.AddConsumer<BannerCreatedEventConsumer>();
                x.AddConsumer<BannerDeletedEventConsumer>();
                x.AddConsumer<BannerUpdatedEventConsumer>();

                //Bilgi Consumers
                x.AddConsumer<BilgiCreatedEventConsumer>();
                x.AddConsumer<BilgiDeletedEventConsumer>();
                x.AddConsumer<BilgiUpdatedEventConsumer>();

                //Birim Consumers
                x.AddConsumer<BirimCreatedEventConsumer>();
                x.AddConsumer<BirimDeletedEventConsumer>();
                x.AddConsumer<BirimUpdatedEventConsumer>();

                //Duyuru Consumers
                x.AddConsumer<DuyuruCreatedEventConsumer>();
                x.AddConsumer<DuyuruDeletedEventConsumer>();
                x.AddConsumer<DuyuruUpdatedEventConsumer>();

                //Etkinlik Consumers
                x.AddConsumer<EtkinlikCreatedEventConsumer>();
                x.AddConsumer<EtkinlikDeletedEventConsumer>();
                x.AddConsumer<EtkinlikUpdatedEventConsumer>();

                //Haber Consumers
                x.AddConsumer<HaberCreatedEventConsumer>();
                x.AddConsumer<HaberDeletedEventConsumer>();
                x.AddConsumer<HaberUpdatedEventConsumer>();

                //Menu Consumers
                x.AddConsumer<MenuChangedEventConsumer>();

                //PersonelTip Consumers
                x.AddConsumer<PersonelTipChangedEventConsumer>();

                //Popup Consumers
                x.AddConsumer<PopupChangedEventConsumer>();

                //SertifikaParmakIzi Consumers
                x.AddConsumer<SertifikaParmakIziChangedConsumer>();

                //SikcaSorulanSoru Consumers
                x.AddConsumer<SikcaSorulanSoruChangedEventConsumer>();

                //SikcaSorulanSoruKategori Consumers
                x.AddConsumer<SikcaSorulanSoruKategoriChangedEventConsumer>();

                //Site Consumers
                x.AddConsumer<SiteChangedEventConsumer>();

                //SiteOzellikleri Consumers
                x.AddConsumer<SiteOzellikleriChangedEventConsumer>();

                //SitePersonel Consumers
                x.AddConsumer<SitePersonelChangedEventConsumer>();

                //Unvan Consumers
                x.AddConsumer<UnvanChangedEventConsumer>();

                //Video Consumers
                x.AddConsumer<VideoChangedEventConsumer>();

                //YoneticiSite Consumers
                x.AddConsumer<YoneticiSiteChangedEventConsumer>();





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
