using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SikcaSorulanSoruKategoriEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.SikcaSorulanSoruKategoriConsumers
{
    public class SikcaSorulanSoruKategoriChangedEventConsumer : IConsumer<SikcaSorulanSoruKategoriChangedEvent>
    {
        private readonly IRedisCacheService _cache;

        public SikcaSorulanSoruKategoriChangedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<SikcaSorulanSoruKategoriChangedEvent> context)
        {
            var message = context.Message;

            var key = $"sikcaSorulanSoruKategori:list";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
