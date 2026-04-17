using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.HaberEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.HaberConsumers
{
    public class HaberUpdatedEventConsumer : IConsumer<HaberUpdatedEvent>
    {
        private readonly IRedisCacheService _cache;

        public HaberUpdatedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<HaberUpdatedEvent> context)
        {
            var message = context.Message;

            var key = $"habers:list:{message.SiteId}:{message.DilId}";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
