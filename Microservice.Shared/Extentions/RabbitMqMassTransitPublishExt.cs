using MassTransit;
using Microservice.Shared.Options;
using Microservice.Shared.Services.RedisServiceItems;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.FileIO;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Shared.Extentions
{
    public static class RabbitMqMassTransitPublishExt
    {
        public static IServiceCollection AddRabbitMqMasstransitPublisherExt(this IServiceCollection services, IConfiguration configuration)
        {


            var busOptions = (configuration.GetSection(nameof(RabbitMqMassTransitBusOption)).Get<RabbitMqMassTransitBusOption>())!;
           
            services.AddMassTransit(configure => {
                configure.UsingRabbitMq((context, cfg) => {
                    cfg.Host(new Uri($"rabbitmq://{busOptions.Address}:{busOptions.Port}"), h => {
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
