using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.UnvanEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.UnvanConsumers
{
    public class UnvanChangedEventConsumer : IConsumer<UnvanChangedEvent>
    {
        private readonly IRedisCacheService _cache;

        public UnvanChangedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<UnvanChangedEvent> context)
        {
            var message = context.Message;

            var key = $"unvans:list";
            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
