using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.HaberEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.HaberConsumers
{
    public class HaberCreatedEventConsumer : IConsumer<HaberCreatedEvent>
    {
        private readonly IRedisCacheService _cache;

        public HaberCreatedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<HaberCreatedEvent> context)
        {
            var message = context.Message;

            var key = $"habers:list:{message.SiteId}:{message.DilId}";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
