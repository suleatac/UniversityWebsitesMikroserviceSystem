using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.MediaFileEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.MediaFileConsumers
{
    public class MediaFileChangedEventConsumer : IConsumer<MediaFileChangedEvent>
    {
        private readonly IRedisCacheService _cache;

        public MediaFileChangedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<MediaFileChangedEvent> context)
        {
            var message = context.Message;

            var key = $"mediafiles:list:{message.SiteId}:{message.DilId}";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
