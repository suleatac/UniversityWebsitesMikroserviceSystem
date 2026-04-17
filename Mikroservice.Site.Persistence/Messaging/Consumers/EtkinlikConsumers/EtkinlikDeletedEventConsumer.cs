using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.EtkinlikEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.EtkinlikConsumers
{
    public class EtkinlikDeletedEventConsumer : IConsumer<EtkinlikDeletedEvent>
    {
        private readonly IRedisCacheService _cache;

        public EtkinlikDeletedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<EtkinlikDeletedEvent> context)
        {
            var message = context.Message;

            var key = $"etkinliks:list:{message.SiteId}:{message.DilId}";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
