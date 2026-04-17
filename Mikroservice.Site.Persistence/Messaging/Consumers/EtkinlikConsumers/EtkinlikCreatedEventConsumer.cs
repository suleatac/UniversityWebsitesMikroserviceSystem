using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.EtkinlikEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.EtkinlikConsumers
{
    public class EtkinlikCreatedEventConsumer : IConsumer<EtkinlikCreatedEvent>
    {
        private readonly IRedisCacheService _cache;

        public EtkinlikCreatedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<EtkinlikCreatedEvent> context)
        {
            var message = context.Message;

            var key = $"etkinliks:list:{message.SiteId}:{message.DilId}";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
