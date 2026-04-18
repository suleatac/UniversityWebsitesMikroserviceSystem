using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.VideoEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.VideoConsumer
{
    public class VideoChangedEventConsumer : IConsumer<VideoChangedEvent>
    {
        private readonly IRedisCacheService _cache;

        public VideoChangedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<VideoChangedEvent> context)
        {
            var message = context.Message;

            var key = $"videos:list:{message.SiteId}:{message.DilId}";
            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
