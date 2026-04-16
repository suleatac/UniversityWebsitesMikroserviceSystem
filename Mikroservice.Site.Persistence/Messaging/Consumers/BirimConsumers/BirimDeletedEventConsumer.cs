using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BirimEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.BirimConsumers
{
    public class BirimDeletedEventConsumer : IConsumer<BirimDeletedEvent>
    {
        private readonly IRedisCacheService _cache;

        public BirimDeletedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<BirimDeletedEvent> context)
        {
            var message = context.Message;

            var key = "birims:list";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
