using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers
{
    public class BannerDeletedEventConsumer : IConsumer<BannerDeletedEvent>
    {
        private readonly IRedisCacheService _cache;

        public BannerDeletedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<BannerDeletedEvent> context)
        {
            var message = context.Message;

            var key = $"banners:list:{message.SiteId}:{message.DilId}";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
