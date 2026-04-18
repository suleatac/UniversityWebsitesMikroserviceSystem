using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BannerEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.BannerConsumers
{
    public class BannerUpdatedEventConsumer : IConsumer<BannerUpdatedEvent>
    {
        private readonly IRedisCacheService _cache;

        public BannerUpdatedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<BannerUpdatedEvent> context)
        {
            var message = context.Message;

            var key = $"banners:list:{message.SiteId}:{message.DilId}";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
