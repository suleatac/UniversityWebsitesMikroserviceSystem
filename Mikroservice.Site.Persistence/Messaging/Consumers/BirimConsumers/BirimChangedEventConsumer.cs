using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BirimEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.BirimConsumers
{
    public class BirimChangedEventConsumer : IConsumer<BirimChangedEvent>
    {
        private readonly IRedisCacheService _cache;

        public BirimChangedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<BirimChangedEvent> context)
        {
            var message = context.Message;

            var key = "birim:list";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
