using MassTransit;
using Microservice.Shared.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Shared.Extentions
{
    public static class RabbitMqMassTransitConsumerExt
    {
        public static IServiceCollection AddRabbitMqMasstransitConsumerExt<TEvent, TConsumer>(
           this IServiceCollection services,
           IConfiguration configuration,
           string queueName)
           where TEvent : class
           where TConsumer : class, IConsumer<TEvent>
        {


            var busOptions = (configuration.GetSection(nameof(RabbitMqMassTransitBusOption)).Get<RabbitMqMassTransitBusOption>())!;

            services.AddMassTransit(x => {
                x.AddConsumer<TConsumer>();

                x.UsingRabbitMq((context, cfg) => {
                    cfg.Host(new Uri($"rabbitmq://{busOptions.Address}:{busOptions.Port}"), h => {
                        h.Username(busOptions.UserName);
                        h.Password(busOptions.Password);
                    });

                    cfg.ReceiveEndpoint(queueName, e => {
                        e.ConfigureConsumer<TConsumer>(context);
                    });
                });
            });

            return services;
        }

        /*Kullanım örneği:
           builder.Services.AddRabbitMqMasstransitConsumerExt<OrderCreatedEvent, OrderCreatedEventConsumer>(builder.Configuration, "basket-microservice.order-created-event.queue");
        */


    }
}
