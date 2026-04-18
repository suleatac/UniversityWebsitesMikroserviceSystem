using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SikcaSorulanSoruEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.SikcaSorulanSoruConsumers
{
    public class SikcaSorulanSoruChangedEventConsumer : IConsumer<SikcaSorulanSoruChangedEvent>
    {
        private readonly IRedisCacheService _cache;

        public SikcaSorulanSoruChangedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<SikcaSorulanSoruChangedEvent> context)
        {
            var message = context.Message;

            var key = $"sikcasorulansoru:list:{message.SiteId}:{message.DilId}";


            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
